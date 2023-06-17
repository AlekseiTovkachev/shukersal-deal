import { useCallback, useEffect, useState } from "react";

import { storesApi } from "../../api/storesApi";
import { ApiError, ApiListData } from "../../types/apiTypes";
import { Store } from "../../types/appTypes";

export const useStores = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [stores, setStores] = useState<Store[]>([]);
  const [error, setError] = useState<string | null>(null);

  const getStores = useCallback(async () => {
    setIsLoading(true);
    storesApi
      .getAll()
      .then((res) => {
        const _stores = res as ApiListData<Store>;
        setIsLoading(false);
        setError(null);
        setStores(_stores);
      })
      .catch((res) => {
        const error = res as ApiError;
        setIsLoading(false);
        setError(error.message ?? "Error.");
        setStores([]);
        console.error("Error while fetching managers: ", error);
      });
  }, []);

  useEffect(() => {
    getStores();
  }, []);

  return {
    isLoading: isLoading,
    stores: stores,
    error: error,
  };
};
