// using System;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
//
// namespace Game.Project.Gameplay.Weapon.Runtime.DamageHandlers
// {
//     [MVCRequiredComposition(typeof(HealthComposition)), Path("Character")]
//     public class HitConsumerComposition : MVCComposition
//     {
//         private const int _defaultMask = 1;
//         private const int _destroyForceMultiplier = 10;
//
//         [SerializeField] private BodyDamageSettings _settings;
//
//         public event Action<HitModel> OnHitWithModel;
//         private HealthComposition _healthComposition;
//         private EffectComposition _effectComposition;
//         private GameObject _gameObject;
//         private int _totalHandlers;
//
//         private List<ArmorHandler> _cachedArmorHandlers;
//         private List<WeakSpotHandler> _cachedWeakSpotHandlers;
//         private Dictionary<SimpleDamageHandler, List<Vector3>> _destroyedArmorDirections = new Dictionary<SimpleDamageHandler, List<Vector3>>();
//         private const float SameSideDotThreshold = 0.5f;
//
//         [Header("Размер коллайдеров так же устанавливается извне например из ArmorSetConfig")]
//         public float hitCollidersScaleMultiplayer = 1f;
//         public float DamageMultiplier { get; private set; } = 1f;
//
//         public void Initialize(GameObject gameObject, int totalHandlers)
//         {
//             _totalHandlers = totalHandlers;
//             _gameObject = gameObject;
//         }
//
//         public void CacheArmorHandlers(ArmorHandler[] armorHandlers)
//         {
//             _cachedArmorHandlers = new List<ArmorHandler>(armorHandlers ?? Array.Empty<ArmorHandler>());
//         }
//
//         public void CacheWeakSpotHandlers(WeakSpotHandler[] weakSpotHandlers)
//         {
//             _cachedWeakSpotHandlers = new List<WeakSpotHandler>(weakSpotHandlers ?? Array.Empty<WeakSpotHandler>());
//         }
//
//         public override void OnModelReady()
//         {
//             base.OnModelReady();
//             _healthComposition = GetComposition<HealthComposition>();
//             _effectComposition = GetComposition<EffectComposition>();
//         }
//
//         public void ProcessHit(SimpleDamageHandler handler, HitModel model)
//         {
//             if (_healthComposition.CurrentHealth.Value <= 0)
//             {
//                 return;
//             }
//
//             OnHitWithModel?.Invoke(model);
//
//             var baseDamage = model.Damage * DamageMultiplier * GetDamageMultiplier(handler);
//             if (model.SpreadingDamage)
//             {
//                 baseDamage /= _totalHandlers;
//             }
//
//             float finalDamage = baseDamage;
//             var armorHandler = FindArmorHandler(handler, model.Point);
//             
//             if (armorHandler != null)
//             {
//                 if (!armorHandler.IsDestroyed)
//                 {
//                     finalDamage = armorHandler.TakeDamage(baseDamage, model.ArmorIgnore, model.Point, model.Direction);
//
//                     if (armorHandler.IsDestroyed)
//                     {
//                         DetachArmor(armorHandler);
//                     }
//                 }
//             }
//
//             float weakSpotMultiplier = 1f;
//             if (armorHandler == null || armorHandler.IsDestroyed)
//             {
//                 weakSpotMultiplier = FindWeakSpotMultiplier(handler, model.Point);
//                 if (weakSpotMultiplier > 1f)
//                 {
//                     finalDamage *= weakSpotMultiplier;
//                     Debug.Log("<color=orange>Попал в супер слот!</color>");
//                 }
//             }
//
//             // Debug.Log($"[HitConsumerComposition] ProcessHit: bodyPart={handler.Part}, baseDamage={baseDamage}, handler={handler.gameObject.name}, weakSpotMultiplier: {weakSpotMultiplier}", handler.gameObject);
//             // Debug.Log($"[HitConsumerComposition] ProcessHit: armorPart={armorHandler?.name}, boneName: {armorHandler?.ArmorBone.name},", handler.gameObject);
//
//             ApplyDamage(finalDamage, model.SourceModel);
//             var isAlive = _healthComposition.CurrentHealth.Value > 0;
//             if (isAlive)
//             {
//                 if (_effectComposition != null)
//                 {
//                     StatusEffectSettings effectSettings = null;
//                     if (_effectComposition.HasEffects && model.Reaction != null)
//                     {
//                         effectSettings = model.Reaction;
//                     }
//                     else if (model.Effect != null)
//                     {
//                         effectSettings = model.Effect;
//                     }
//
//                     if (effectSettings != null)
//                     {
//                         var effectModel = new ProcessEffectModel(effectSettings, model.CharacterGuid, model.WeaponGuid);
//                         _effectComposition.Process(effectModel);
//                     }
//                 }
//             }
//             else
//             {
//                 OnDie(handler, model);
//             }
//         }
//
//         public void ApplyDamage(float damage, MVCModel source = null)
//         {
//             _healthComposition.Damage(damage, source);
//
//             var isAlive = _healthComposition.CurrentHealth.Value > 0;
//             if (!isAlive)
//             {
//                 var jointsManager = _gameObject.GetComponent<JointsManager>();
//                 if (jointsManager != null)
//                 {
//                     if (jointsManager.CurrentState == ERagdollState.Ragdolled)
//                     {
//                         return;
//                     }
//
//                     jointsManager.JointsSetState(ERagdollState.Ragdolled);
//                 }
//
//                 var destroyableView = _gameObject.GetComponent<DestroyableView>();
//                 if (destroyableView != null)
//                 {
//                     destroyableView.OnDestroyed();
//                 }
//             }
//         }
//
//         private float GetDamageMultiplier(SimpleDamageHandler handler)
//         {
//             var part = handler.Part;
//             var settings = _settings.Values.FirstOrDefault(x => x.Part == part);
//             if (settings != null)
//             {
//                 return settings.Multiplier;
//             }
//
//             return _settings.DefaultValue;
//         }
//
//         private void OnDie(SimpleDamageHandler handler, HitModel model)
//         {
//             var direction = model.Direction;
//             var hitPoint = model.Point;
//             var dir = direction + Vector3.up * 0.5f;
//
//             var jointsManager = _gameObject.GetComponent<JointsManager>();
//             if (jointsManager != null)
//             {
//                 jointsManager.AddForceAtPosition(
//                     dir.normalized,
//                     hitPoint + Vector3.up * 0.2f,
//                     model.ForcePower,
//                     model.ForceRadius);
//             }
//
//             var destroyableView = _gameObject.GetComponent<DestroyableView>();
//             if (destroyableView != null)
//             {
//                 var count = ShooterRayCastHelper.OverlapSphereNonAlloc(
//                     model.Point,
//                     model.ForceRadius,
//                     out var colliders,
//                     layerMask: _defaultMask);
//                 for (int i = 0; i < count; ++i)
//                 {
//                     var rigidBody = colliders[i].GetComponent<Rigidbody>();
//                     if (rigidBody != null)
//                     {
//                         rigidBody.solverIterations = 2;
//                         rigidBody.solverVelocityIterations = 1;
//                         rigidBody.maxLinearVelocity = 10;
//                         rigidBody.AddExplosionForce(
//                             model.ForcePower * _destroyForceMultiplier,
//                             model.Point,
//                             model.ForceRadius,
//                             0f,
//                             ForceMode.Impulse);
//                     }
//                 }
//             }
//         }
//
//         private void DetachArmor(ArmorHandler handler)
//         {
//             if (handler == null)
//             {
//                 Debug.LogError("[HitConsumerComposition] DetachArmor: handler is null");
//                 return;
//             }
//
//             var armorGameObject = handler.gameObject;
//             if (armorGameObject == null)
//             {
//                 Debug.LogError("[HitConsumerComposition] DetachArmor: armorGameObject is null");
//                 return;
//             }
//
//             if (armorGameObject.TryGetComponent<ArmorHandler>(out var armorHandler))
//             {
//                 armorHandler.PlaySparks();
//             }
//
//             Debug.Log($"[HitConsumerComposition] === DETACHING ARMOR: {armorGameObject.name} ===");
//
//             var damageHandler = handler.GetDamageHandler();
//             if (damageHandler != null)
//             {
//                 var direction = (armorGameObject.transform.position - damageHandler.transform.position).normalized;
//                 if (!_destroyedArmorDirections.TryGetValue(damageHandler, out var list))
//                 {
//                     list = new List<Vector3>();
//                     _destroyedArmorDirections[damageHandler] = list;
//                 }
//                 list.Add(direction);
//             }
//
//             armorGameObject.transform.SetParent(null);
//
//             MeshFilter meshFilter = armorGameObject.GetComponent<MeshFilter>();
//             if (meshFilter == null)
//             {
//                 Debug.LogError($"[HitConsumerComposition] No MeshFilter on {armorGameObject.name}");
//                 return;
//             }
//
//             if (meshFilter.sharedMesh == null)
//             {
//                 Debug.LogError($"[HitConsumerComposition] MeshFilter.sharedMesh is null on {armorGameObject.name}");
//                 return;
//             }
//
//             Rigidbody rb = armorGameObject.GetComponent<Rigidbody>();
//             if (rb == null)
//             {
//                 rb = armorGameObject.AddComponent<Rigidbody>();
//                 Debug.Log($"[HitConsumerComposition] Added Rigidbody to {armorGameObject.name}");
//             }
//
//             rb.mass = 5f;
//             rb.linearDamping = 0.5f;
//             rb.angularDamping = 0.5f;
//             rb.isKinematic = false;
//             rb.useGravity = true;
//
//             MeshCollider meshCollider = armorGameObject.GetComponent<MeshCollider>();
//             if (meshCollider == null)
//             {
//                 meshCollider = armorGameObject.AddComponent<MeshCollider>();
//                 Debug.Log($"[HitConsumerComposition] Added MeshCollider to {armorGameObject.name}");
//             }
//
//             meshCollider.convex = true;
//             meshCollider.sharedMesh = meshFilter.sharedMesh;
//             meshCollider.enabled = true;
//
//             Vector3 forceDirection = (armorGameObject.transform.position - _gameObject.transform.position).normalized;
//             if (forceDirection.magnitude < 0.1f)
//             {
//                 forceDirection = UnityEngine.Random.onUnitSphere;
//             }
//
//             float forcePower = 5f;
//             rb.AddForce(forceDirection * forcePower, ForceMode.Impulse);
//             rb.AddTorque(UnityEngine.Random.insideUnitSphere * 3f, ForceMode.Impulse);
//
//             handler.enabled = false;
//
//             UnityEngine.Object.Destroy(armorGameObject, 30f);
//         }
//
//         private float FindWeakSpotMultiplier(SimpleDamageHandler damageHandler, Vector3 hitPoint)
//         {
//             if (_cachedWeakSpotHandlers == null) return 1f;
//
//             float maxMultiplier = 1f;
//             foreach (var weakSpot in _cachedWeakSpotHandlers)
//             {
//                 if (weakSpot == null || weakSpot.GetDamageHandler() != damageHandler)
//                     continue;
//                 if (weakSpot.ContainsPoint(hitPoint) && weakSpot.DamageMultiplier > maxMultiplier)
//                     maxMultiplier = weakSpot.DamageMultiplier;
//             }
//             return maxMultiplier;
//         }
//
//         private ArmorHandler FindArmorHandler(SimpleDamageHandler damageHandler, Vector3 hitPoint)
//         {
//             if (damageHandler == null)
//             {
//                 return null;
//             }
//
//             if (_destroyedArmorDirections.TryGetValue(damageHandler, out var destroyedDirs))
//             {
//                 var hitDirection = (hitPoint - damageHandler.transform.position).normalized;
//                 foreach (var dir in destroyedDirs)
//                 {
//                     if (Vector3.Dot(hitDirection, dir) >= SameSideDotThreshold)
//                         return null;
//                 }
//             }
//
//             ArmorHandler closestArmor = null;
//             float minDistance = float.MaxValue;
//
//             foreach (var armorHandler in _cachedArmorHandlers)
//             {
//                 if (armorHandler == null || armorHandler.GetDamageHandler() != damageHandler || armorHandler.IsDestroyed)
//                     continue;
//
//                 float distance = Vector3.Distance(hitPoint, armorHandler.transform.position);
//                 if (distance < minDistance)
//                 {
//                     minDistance = distance;
//                     closestArmor = armorHandler;
//                 }
//             }
//
//             return closestArmor;
//         }
//
//         internal void MultiplyCollidersScale(float collidersScaleMultiplayer)
//         {
//             hitCollidersScaleMultiplayer *= collidersScaleMultiplayer;
//         }
//
//         public void SetDamageMultiplier(float multiplier)
//         {
//             DamageMultiplier = multiplier;
//         }
//     }
// }
