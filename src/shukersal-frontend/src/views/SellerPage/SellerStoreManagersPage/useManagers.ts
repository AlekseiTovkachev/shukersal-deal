// {
//     "id": 6,
//     "storeId": 4,
//     "memberId": 1,
//     "parentManagerId": null,
//     "storePermissions": [
//       {
//         "id": 6,
//         "permissionType": 0,
//         "storeManagerId": 6
//       }
//     ],
//     "childManagers": []
// }

import { useState, useEffect, useCallback } from "react";
import { StoreManager } from "../../../types/appTypes";
import { storeManagersApi } from "../../../api/storesApi";
import { ApiError } from "../../../types/apiTypes";

export const useManagers = (storeId: number) => {
  const [rootManager, setRootManager] = useState<StoreManager | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const getStoreManagers = useCallback(async (_storeId: number) => {
    setIsLoading(true);
    storeManagersApi
      .getRoot(_storeId)
      .then((res) => {
        const manager = res as StoreManager;
        setIsLoading(false);
        setError(null);
        setRootManager(manager);
      })
      .catch((res) => {
        const error = res as ApiError;
        setIsLoading(false);
        setError(error.message ?? "Error.");
        setRootManager(null);
        console.error("Error while fetching managers: ", error);
      });
  }, []);

  useEffect(() => {
    getStoreManagers(storeId);
  }, []);

  return {
    isLoading: isLoading,
    error: error,
    rootManager: rootManager,

    getStoreManagers: () => getStoreManagers(storeId),
    // addProduct: addProductCallback,
    // editProduct: editProductCallback,
    // updateStore: updateStoreCallback,
    // deleteStore: deleteStoreCallback,
  };
};
