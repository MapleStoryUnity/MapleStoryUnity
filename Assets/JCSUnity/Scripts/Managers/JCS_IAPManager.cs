//#define IAP_ENABLED
#if IAP_ENABLED
/**
 * $File: JCS_IAPManager.cs $
 * $Date: 2019-09-24 15:32:34 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2019 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using UnityEngine.Purchasing;

namespace JCSUnity
{
    /// <summary>
    /// In-App-Purchase manager.
    /// </summary>
    public class JCS_IAPManager : MonoBehaviour
        , IStoreListener
    {
        /* Variables */

        // The Unity Purchasing system.
        private static IStoreController STORE_CONTROLLER = null;
        // The store-specific Purchasing subsystems.
        private static IExtensionProvider STORE_EXT_PROVIDER = null;

        // TODO: Design framework interface for this...
        public string mConsumableProductId = "product_id_01";
        public string mNonConsumableProductId = "product_id_02";

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            if (STORE_CONTROLLER != null)
                return;
            InitializePurchasing();
        }

        public void InitializePurchasing()
        {
            if (IsInitialized())
                return;

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Add product here. 
            {
                // Consumable
                builder.AddProduct(mConsumableProductId, ProductType.Consumable);

                // Non-Consumable
                builder.AddProduct(mNonConsumableProductId, ProductType.NonConsumable);
            }

            // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration
            // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
            UnityPurchasing.Initialize(this, builder);
        }

        /// <summary>
        /// Only work in iOS.
        /// </summary>
        public void RestorePurchases()
        {
            if (!IsInitialized())
                return;

            // Check if on iOS.
            if (Application.platform != RuntimePlatform.IPhonePlayer &&
                Application.platform != RuntimePlatform.OSXPlayer)
                return;

            Debug.Log("RestorePurchases started ...");

            var apple = STORE_EXT_PROVIDER.GetExtension<IAppleExtensions>();

            apple.RestoreTransactions((result) =>
            {
                // TODO: Restore puschased items...
            });
        }

        private void BuyProductID(string productId)
        {
            if (IsInitialized())
                return;

            Product product = STORE_CONTROLLER.products.WithID(productId);

            if (product == null || !product.availableToPurchase)
                return;

            STORE_CONTROLLER.InitiatePurchase(product);
        }

        private bool BuyingProduct(PurchaseEventArgs args, string id)
        {
            return String.Equals(args.purchasedProduct.definition.id, id, StringComparison.Ordinal);
        }

        private bool HasPurchased(string productId)
        {
            Product product = STORE_CONTROLLER.products.WithID(productId);
            return product.hasReceipt;
        }

        /// <summary>
        /// Check for initialization.
        /// </summary>
        /// <returns></returns>
        private bool IsInitialized()
        {
            // Only say we are initialized if both the Purchasing references are set.
            return STORE_CONTROLLER != null && STORE_EXT_PROVIDER != null;
        }


        //  
        // --- IStoreListener
        //

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            STORE_CONTROLLER = controller;
            STORE_EXT_PROVIDER = extensions;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // TODO: Failed initialization...
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            if (BuyingProduct(args, mConsumableProductId))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // TODO: After buying `mConsumableProductId`.
            }
            else
            {
                Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            }

            return PurchaseProcessingResult.Complete;
        }


        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            // TODO: Failed purchasing...
        }
    }
}
#endif
