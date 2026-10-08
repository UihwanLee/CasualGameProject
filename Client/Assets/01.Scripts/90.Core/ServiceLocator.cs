using System;
using System.Collections.Generic;
using UnityEngine;

// 인터페이스 타입으로 서비스를 등록하고 꺼내 쓰는 서비스 로케이터
// 등록은 GameBootstrapper에서만 하고, 서비스끼리는 생성자로 주입받는다.
// Resolve는 GameBootstrapper와 MonoBehaviour(엔티티)에서만 사용한다.
// 한 인스턴스는 하나의 인터페이스로만 등록한다. (Clear에서 중복 Dispose 방지)
public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();
    private static readonly List<object> bindOrder = new List<object>();     // 해제를 등록의 역순으로 하기 위한 목록

    /// <summary>
    /// 서비스 등록 (인터페이스 타입만, 중복 등록 불가)
    /// </summary>
    /// <param name="instance">등록할 서비스</param>
    public static void Bind<T>(T instance) where T : class
    {
        Type type = typeof(T);

        if (!type.IsInterface)
            throw new ArgumentException($"{type.Name}은(는) 인터페이스가 아닙니다. 인터페이스 타입으로만 등록할 수 있습니다.");

        if (instance == null)
            throw new ArgumentNullException(nameof(instance), $"{type.Name}에 null을 등록할 수 없습니다.");

        if (services.ContainsKey(type))
            throw new InvalidOperationException($"{type.Name}이(가) 이미 등록되어 있습니다.");

        services.Add(type, instance);
        bindOrder.Add(instance);
    }

    /// <summary>
    /// 서비스 조회 (등록되지 않았으면 예외)
    /// </summary>
    public static T Resolve<T>() where T : class
    {
        if (services.TryGetValue(typeof(T), out object instance))
            return (T)instance;

        throw new InvalidOperationException($"{typeof(T).Name}이(가) 등록되지 않았습니다.");
    }

    /// <summary>
    /// 서비스 조회 (등록되지 않았으면 false)
    /// </summary>
    public static bool TryResolve<T>(out T instance) where T : class
    {
        if (services.TryGetValue(typeof(T), out object obj))
        {
            instance = (T)obj;
            return true;
        }

        instance = null;
        return false;
    }

    /// <summary>
    /// 등록의 역순으로 IDisposable 서비스를 해제하고 전부 제거
    /// </summary>
    public static void Clear()
    {
        for (int i = bindOrder.Count - 1; i >= 0; i--)
        {
            if (bindOrder[i] is IDisposable disposable)
                disposable.Dispose();
        }

        services.Clear();
        bindOrder.Clear();
    }

    // 도메인 리로드를 끈 상태(Enter Play Mode Options)에서도 이전 플레이의 등록이 남지 않게 한다
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnPlay()
    {
        services.Clear();
        bindOrder.Clear();
    }
}
