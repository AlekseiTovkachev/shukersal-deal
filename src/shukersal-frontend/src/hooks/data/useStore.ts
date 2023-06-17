import { useCallback, useEffect, useState } from "react";

import { storeProductsApi, storesApi } from "../../api/storesApi";
import { ApiError, ApiListData } from "../../types/apiTypes";
import { Product, Store } from "../../types/appTypes";

export const useStore = (storeId: number) => {
  const [isLoading, setIsLoading] = useState(false);
  const [store, setStore] = useState<Store | null>(null);
  const [storeProducts, setStoreProducts] = useState<Product[]>([]);
  const [error, setError] = useState<string | null>(null);

  const getStore = useCallback(async () => {
    setIsLoading(true);
    storesApi
      .get(storeId)
      .then((res) => {
        const _store = res as Store;
        setIsLoading(false);
        setError(null);
        setStore(_store);
      })
      .catch((res) => {
        const error = res as ApiError;
        setIsLoading(false);
        setError(error.message ?? "Error.");
        setStore(null);
        console.error("Error while fetching managers: ", error);
      });
  }, [setError, setStore, setIsLoading, storeId]);

  const getStoreProducts = useCallback(async () => {
    setIsLoading(true);
    storeProductsApi
      .getAll(storeId)
      .then((res) => {
        const _products = res as ApiListData<Product>;
        setIsLoading(false);
        setError(null);
        setStoreProducts(_products);
      })
      .catch((res) => {
        const error = res as ApiError;
        setIsLoading(false);
        setError(error.message ?? "Error.");
        setStoreProducts([]);
        console.error("Error while fetching managers: ", error);
      });
  }, [setError, setStore, setIsLoading, storeId]);

  return {
    getStore: getStore,
    getStoreProducts: getStoreProducts,
    isLoading: isLoading,
    store: store,
    storeProducts: storeProducts,
    error: error,
  };
};
