using App.Common.Utilities.UtilityUnity.Runtime.Extensions;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Common.Utilities.UtilityUnity.Runtime
{
    public static class TweenHelper
    {
        public static Tweener TweenDOMoveX(Transform target, float endValue, float duration, bool snapping = false)
        {
            return DOTween.To(
                    () =>
                    {
                        if (target != null)
                        {
                            return target.position;
                        }
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        return new Vector3(endValue, 0, 0);
                    },
                    x =>
                    {
                        if (target != null)
                        {
                            target.position = x;
                        }
                        else
                        {
                            Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        }
                    },
                    new Vector3(endValue, 0, 0),
                    duration)
                .SetOptions(AxisConstraint.X, snapping).SetTarget(target);
        }


        public static Tween TweenDoScale(Transform tweenTransform, Vector3 endValue, float duration)
        {
            return DOTween
                .To(
                    () =>
                    {
                        if (tweenTransform != null)
                        {
                            return tweenTransform.localScale;
                        }
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        return endValue;
                    },
                    x =>
                    {
                        if (tweenTransform != null)
                        {
                            tweenTransform.localScale = x;
                        }
                        else
                        {
                            Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        }
                    },
                    endValue,
                    duration)
                .SetTarget(tweenTransform); 
        }

        public static Tween TweenDoScale(Transform tweenTransform, Vector3 startValue,  Vector3 endValue, float duration)
        {
            return DOTween
                .To(
                    () =>
                    {
                        if (tweenTransform != null)
                        {
                            return startValue;
                        }
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        return startValue;
                    },
                    x =>
                    {
                        if (tweenTransform != null)
                        {
                            tweenTransform.localScale = x;
                        }
                        else
                        {
                            Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        }
                    },
                    endValue,
                    duration);
        }

        public static Tweener TweenDOFade(Material target, float endValue, float duration)
        {
            return DOTween
                .ToAlpha(
                    () =>
                    {
                        if (target != null)
                        {
                            return target.color;
                        }
                        Debug.LogError("опять material кто-то убил, а твину нет");
                        return new Color(0, 0, 0, endValue);
                    },
                    x =>
                    {
                        if (target != null)
                        {
                            target.color = x;
                        }
                        else
                        {
                            Debug.LogError("опять material кто-то убил, а твину нет");
                        }
                    },
                    endValue,
                    duration)
                .SetTarget(target);
        }

        public static Tweener TweenDORotate(Transform target, Vector3 endValue, float duration, RotateMode mode = RotateMode.Fast)
        {        
            TweenerCore<Quaternion, Vector3, QuaternionOptions> t = DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return target.rotation;
                    }
                    Debug.LogError("опять трансформ кто-то убил, а твину нет");
                    return Quaternion.identity;
                },
                x =>
                {
                    if (target != null)
                    {
                        target.rotation = x;
                    }
                    else
                    {
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                    }
                },
                endValue,
                duration);
            t.SetTarget(target);
            t.plugOptions.rotateMode = mode;
            return t;
        }
        
        public static Tweener TweenDORotateZ(Transform target, float endValue, float duration)
        {        
            TweenerCore<float,float,FloatOptions> tween = DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return target.rotation.z;
                    }
                    Debug.LogError("опять трансформ кто-то убил, а твину нет");
                    return endValue;
                },
                x =>
                {
                    if (target != null)
                    {
                        target.SetEulerRotateZ(x);
                    }
                    else
                    {
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                    }
                },
                endValue,
                duration);
            tween.SetTarget(target);
            return tween;
        }
        
        public static Tweener TweenDOLocalRotateZ(Transform target, float endValue, float duration)
        {        
            TweenerCore<float,float,FloatOptions> tween = DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return target.localRotation.z;
                    }
                    Debug.LogError("опять трансформ кто-то убил, а твину нет");
                    return endValue;
                },
                x =>
                {
                    if (target != null)
                    {
                        target.SetLocalEulerRotateZ(x);
                    }
                    else
                    {
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                    }
                },
                endValue,
                duration);
            tween.SetTarget(target);
            return tween;
        }

        public static Tweener TweenDORotate(Transform target, Vector3 startValue, Vector3 rotateValue, float duration, RotateMode mode = RotateMode.Fast)
        {
            Quaternion sartQuaternion = Quaternion.Euler(startValue);
            Vector3 endValue = startValue + rotateValue;
            TweenerCore<Quaternion, Vector3, QuaternionOptions> t = DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return sartQuaternion;
                    }
                    Debug.LogError("опять трансформ кто-то убил, а твину нет");
                    return sartQuaternion;
                },
                x =>
                {
                    if (target != null)
                    {
                        target.rotation = x;
                    }
                    else
                    {
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                    }
                },
                endValue,
                duration);
            t.plugOptions.rotateMode = mode;
            t.onComplete = () => target.rotation = sartQuaternion;
            return t;
        }


        public static Tweener TweenDOLocalMove(Transform target, Vector3 endValue, float duration, bool snapping = false)
        {        
            return DOTween.To(
                    () =>
                    {
                        if (target != null)
                        {
                            return target.localPosition;
                        }
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        return endValue;
                    },
                    x =>
                    {
                        if (target != null)
                        {
                            target.localPosition = x;
                        }
                        else
                        {
                            Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        }
                    },
                    endValue,
                    duration)
                .SetOptions(snapping).SetTarget(target);
        }

        public static Tween TweenDoScale(Transform tweenTransform, float endValue, float duration)
        {
            return TweenDoScale(tweenTransform, Vector3.one * endValue, duration);
        }

        public static Tweener TweenDOFade(SpriteRenderer target, float endValue, float duration, string name = "")
        {
            return DOTween
                .ToAlpha(
                    () =>
                    {
                        if (target != null)
                        {
                            return target.color;
                        }
                        Debug.LogError($"опять SpriteRenderer кто-то убил, а твину нет {name}");
                        return new Color(0, 0, 0, endValue);
                    },
                    x =>
                    {
                        if (target != null)
                        {
                            target.color = x;
                        }
                        else
                        {
                            Debug.LogError($"опять SpriteRenderer кто-то убил, а твину нет {name}");
                        }
                    },
                    endValue,
                    duration)
                .SetTarget(target);
        }

        public static Tweener TweenDOLocalMove(Transform target, float endValue, float duration, bool snapping = false)
        {
            return TweenDOLocalMove(target, Vector3.one * endValue, duration, snapping);
        }

        public static Tweener TweenDOMove(Transform target, Vector3 endValue, float duration, bool snapping = false)
        {        
            return DOTween.To(() =>
                    {
                        if (target != null)
                        {
                            return target.position;
                        }
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        return endValue;
                    },
                    x =>
                    {
                        if (target != null)
                        {
                            target.position = x;
                        }
                        else
                        {
                            Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        }
                    },
                    endValue,
                    duration)
                .SetOptions(snapping).SetTarget(target);
        }

        public static Tweener TweenDOScaleX(Transform target, float endValue, float duration)
        {
            return DOTween.To(
                    () =>
                    {
                        if (target != null)
                        {
                            return target.localScale;
                        }
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        return Vector3.one * endValue;
                    },
                    x =>
                    {
                        if (target != null)
                        {
                            target.localScale = x;
                        }
                        else
                        {
                            Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        }
                    },
                    new Vector3(endValue, 0, 0),
                    duration)
                .SetOptions(AxisConstraint.X)
                .SetTarget(target);
        }

        public static Tweener TweenDOScaleY(Transform target, float endValue, float duration)
        {
            return DOTween.To(
                    () =>
                    {
                        if (target != null)
                        {
                            return target.localScale;
                        }
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        return Vector3.one * endValue;
                    },
                    x =>
                    {
                        if (target != null)
                        {
                            target.localScale = x;
                        }
                        else
                        {
                            Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        }
                    },
                    new Vector3(0, endValue, 0),
                    duration)
                .SetOptions(AxisConstraint.Y)
                .SetTarget(target);
        }

        public static TweenerCore<float, float, FloatOptions> TweenDOFade(CanvasGroup target, float endValue, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return target.alpha;
                    }
                    Debug.LogError("опять CanvasGroup кто-то убил, а твину нет");
                    return endValue;
                },
                x =>
                {
                    if (target != null)
                    {
                        target.alpha = x;
                    }
                    else
                    {
                        Debug.LogError("опять CanvasGroup кто-то убил, а твину нет");
                    }
                },
                endValue,
                duration);
            t.SetTarget(target);
            return t;
        }

        public static TweenerCore<Color, Color, ColorOptions> TweenDOFade(Image target, float endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> t = DOTween.ToAlpha(
                () =>
                {
                    if (target != null)
                    {
                        return target.color;
                    }
                    Debug.LogError("опять Image кто-то убил, а твину нет");
                    return new Color(0, 0, 0, endValue);
                },
                x =>
                {
                    if (target != null)
                    {
                        target.color = x;
                    }
                    else
                    {
                        Debug.LogError("опять Image кто-то убил, а твину нет");
                    }
                },
                endValue,
                duration);
            t.SetTarget(target);
            return t;
        }
    
        public static TweenerCore<float, float, FloatOptions> TweenDOFade(TMP_Text target, float endValue, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return target.alpha;
                    }
                    Debug.LogError("опять Image кто-то убил, а твину нет");
                    return endValue;
                },
                x =>
                {
                    if (target != null)
                    {
                        target.alpha = x;
                    }
                    else
                    {
                        Debug.LogError("опять Image кто-то убил, а твину нет");
                    }
                },
                endValue,
                duration);
            t.SetTarget(target);
            return t;
        }

        public static TweenerCore<Color, Color, ColorOptions> TweenDOFade(Text target, float endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> t = DOTween.ToAlpha(
                () =>
                {
                    if (target != null)
                    {
                        return target.color;
                    }
                    Debug.LogError("опять Text кто-то убил, а твину нет");
                    return new Color(0, 0, 0, endValue);
                },
                x =>
                {
                    if (target != null)
                    {
                        target.color = x;
                    }
                    else
                    {
                        Debug.LogError("опять Text кто-то убил, а твину нет");
                    }
                },
                endValue,
                duration);
            t.SetTarget(target);
            return t;
        }

        public static Tweener TweenDOLocalMoveX(Transform target, float endValue, float duration, bool snapping = false)
        {
            return DOTween.To(
                    () =>
                    {
                        if (target != null)
                        {
                            return target.localPosition;
                        }
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        return new Vector3(endValue, 0, 0);
                    },
                    x =>
                    {
                        if (target != null)
                        {
                            target.localPosition = x;
                        }
                        else
                        {
                            Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        }
                    },
                    new Vector3(endValue, 0, 0),
                    duration)
                .SetOptions(AxisConstraint.X, snapping).SetTarget(target);
        }

        public static Tweener TweenDOMoveY(Transform target, float endValue, float duration, bool snapping = false)
        {
            return DOTween.To(
                    () =>
                    {
                        if (target != null)
                        {
                            return target.position;
                        }
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        return new Vector3(0, endValue, 0);
                    },
                    x =>
                    {
                        if (target != null)
                        {
                            target.position = x;
                        }
                        else
                        {
                            Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        }
                    },
                    new Vector3(0, endValue, 0),
                    duration)
                .SetOptions(AxisConstraint.Y, snapping).SetTarget(target);
        }

        public static TweenerCore<Vector2, Vector2, VectorOptions> TweenDOAnchorPos(RectTransform target, Vector2 endValue, float duration, bool snapping = false)
        {        
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return target.anchoredPosition;
                    }
                    Debug.LogError("опять RectTransform кто-то убил, а твину нет");
                    return endValue;
                },
                x =>
                {
                    if (target != null)
                    {
                        target.anchoredPosition = x;
                    }
                    else
                    {
                        Debug.LogError("опять RectTransform кто-то убил, а твину нет");
                    }
                },
                endValue,
                duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }

        public static TweenerCore<Vector2, Vector2, VectorOptions> TweenDOSizeDelta(RectTransform target, Vector2 endValue, float duration, bool snapping = false)
        {       
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return target.sizeDelta;
                    }
                    Debug.LogError("опять RectTransform кто-то убил, а твину нет");
                    return endValue;
                },
                x =>
                {
                    if (target != null)
                    {
                        target.sizeDelta = x;
                    }
                    else
                    {
                        Debug.LogError("опять RectTransform кто-то убил, а твину нет");
                    }
                },
                endValue,
                duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }

        public static TweenerCore<Color, Color, ColorOptions> TweenDOColor(SpriteRenderer target, Color endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> t = DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return target.color;
                    }
                    Debug.LogError("опять SpriteRenderer кто-то убил, а твину нет");
                    return endValue;
                },
                x =>
                {
                    if (target != null)
                    {
                        target.color = x;
                    }
                    else
                    {
                        Debug.LogError("опять SpriteRenderer кто-то убил, а твину нет");
                    }
                },
                endValue,
                duration);
            t.SetTarget(target);
            return t;
        }

        public static TweenerCore<Color, Color, ColorOptions> TweenDOColor(Image target, Color endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> t = DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return target.color;
                    }
                    Debug.LogError("опять Image кто-то убил, а твину нет");
                    return endValue;
                },
                x =>
                {
                    if (target != null)
                    {
                        target.color = x;
                    }
                    else
                    {
                        Debug.LogError("опять Image кто-то убил, а твину нет");
                    }
                },
                endValue,
                duration);
            t.SetTarget(target);
            return t;
        }

        public static Tweener TweenDOLocalRotate(Transform target, Vector3 endValue, float duration, RotateMode mode = RotateMode.Fast)
        {
            TweenerCore<Quaternion, Vector3, QuaternionOptions> t = DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return target.localRotation;
                    }
                    Debug.LogError("опять Transform кто-то убил, а твину нет");
                    return Quaternion.identity;
                },
                x =>
                {
                    if (target != null)
                    {
                        target.localRotation = x;
                    }
                    else
                    {
                        Debug.LogError("опять Transform кто-то убил, а твину нет");
                    }
                },
                endValue,
                duration);
            t.SetTarget(target);
            t.plugOptions.rotateMode = mode;
            return t;
        }

        public static TweenerCore<Vector2, Vector2, VectorOptions> TweenDOPivotX(this RectTransform target, float endValue, float duration)
        {
            Debug.LogError("TweenDOPivotX");
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return target.pivot;
                    }
                    Debug.LogError("опять RectTransform кто-то убил, а твину нет");
                    return new Vector3(endValue, 0, 0);
                },
                x =>
                {
                    if (target != null)
                    {
                        target.pivot = x;
                    }
                    else
                    {
                        Debug.LogError("опять RectTransform кто-то убил, а твину нет");
                    }
                },
                new Vector2(endValue, 0),
                duration);
            t.SetOptions(AxisConstraint.X).SetTarget(target);
            return t;
        }

        public static TweenerCore<Vector2, Vector2, VectorOptions> TweenDOAnchorPosX(this RectTransform target, float endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return target.anchoredPosition;
                    }
                    Debug.LogError("опять RectTransform кто-то убил, а твину нет");
                    return new Vector2(endValue, 0);
                },
                x =>
                {
                    if (target != null)
                    {
                        target.anchoredPosition = x;
                    }
                    else
                    {
                        Debug.LogError("опять RectTransform кто-то убил, а твину нет");
                    }
                },
                new Vector2(endValue, 0),
                duration);
            t.SetOptions(AxisConstraint.X, snapping).SetTarget(target);
            return t;
        }
    
        public static TweenerCore<Vector2, Vector2, VectorOptions> TweenDOAnchorPosY(this RectTransform target, float endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return target.anchoredPosition;
                    }
                    Debug.LogError("опять RectTransform кто-то убил, а твину нет");
                    return new Vector2(0, endValue);
                },
                y =>
                {
                    if (target != null)
                    {
                        target.anchoredPosition = y;
                    }
                    else
                    {
                        Debug.LogError("опять RectTransform кто-то убил, а твину нет");
                    }
                },
                new Vector2(0, endValue),
                duration);
            t.SetOptions(AxisConstraint.Y, snapping).SetTarget(target);
            return t;
        }
    
        public static Tweener TweenDOSize(RectTransform target, Vector2 endValue, float duration, bool snapping = false)
        {
            return DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return target.sizeDelta;
                    }
                    Debug.LogError("опять трансформ кто-то убил, а твину нет");
                    return endValue;
                },
                x =>
                {
                    if (target != null)
                    {
                        target.sizeDelta = x;
                    }
                    else
                    {
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                    }
                },
                endValue,
                duration).SetEase(Ease.Linear).SetTarget(target);
        }
    
        public static Tweener TweenDOLocalMove(RectTransform target, Vector2 endValue, float duration, bool snapping = false)
        {
            return DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return target.localPosition;
                    }
                    Debug.LogError("опять трансформ кто-то убил, а твину нет");
                    return new Vector3(endValue.x, endValue.y, 0);
                },
                x =>
                {
                    if (target != null)
                    {
                        target.localPosition = x;
                    }
                    else
                    {
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                    }
                },
                new Vector3(endValue.x, endValue.y, 0),
                duration).SetEase(Ease.Linear).SetTarget(target);
        }
    
        public static Tweener TweenDOText(TextMeshProUGUI target, string text, float duration)
        {
            return DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return target.text;
                    }
                    Debug.LogError("опять трансформ кто-то убил, а твину нет");
                    return text;
                },
                x =>
                {
                    if (target != null)
                    {
                        target.text = x;
                    }
                    else
                    {
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                    }
                },
                text,
                duration).SetEase(Ease.Linear);
        }
    
        public static Tweener TweenDOLocalMoveY(Transform target, float endValue, float duration, bool snapping = false)
        {
            return DOTween.To(
                    () =>
                    {
                        if (target != null)
                        {
                            return target.localPosition;
                        }
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        return new Vector3(0, endValue, 0);
                    },
                    x =>
                    {
                        if (target != null)
                        {
                            target.localPosition = x;
                        }
                        else
                        {
                            Debug.LogError("опять трансформ кто-то убил, а твину нет");
                        }
                    },
                    new Vector3(0, endValue, 0),
                    duration)
                .SetOptions(AxisConstraint.Y, snapping).SetTarget(target);
        }
    
        public static Tweener TweenDOFill(Image target, float endValue, float duration, bool snapping = false)
        {
            return DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return target.fillAmount;
                    }
                    Debug.LogError("опять трансформ кто-то убил, а твину нет");
                    return endValue;
                },
                x =>
                {
                    if (target != null)
                    {
                        target.fillAmount = x;
                    }
                    else
                    {
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                    }
                }, endValue, duration).SetTarget(target);
        }
    
        public static Tweener TweenDOSizeDeltaX(RectTransform target, float endValue, float duration, bool snapping = false)
        {
            return DOTween.To(
                () =>
                {
                    if (target != null)
                    {
                        return target.sizeDelta.x;
                    }
                    Debug.LogError("опять трансформ кто-то убил, а твину нет");
                    return endValue;
                },
                x =>
                {
                    if (target != null)
                    {
                        target.sizeDelta = new Vector2(x, target.sizeDelta.y);
                    }
                    else
                    {
                        Debug.LogError("опять трансформ кто-то убил, а твину нет");
                    }
                },
                endValue,
                duration).SetTarget(target);
        }
    }
}
