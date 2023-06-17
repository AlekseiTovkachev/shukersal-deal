import { useCallback, useEffect, useState } from "react";

import { Product } from "../../types/appTypes";
import { storesApi } from "../../api/storesApi";
import { ApiError, ApiListData } from "../../types/apiTypes";

export const useProducts = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [products, setProducts] = useState<Product[]>([]);
  const [error, setError] = useState<string | null>(null);

  const getProducts = useCallback(async () => {
    setIsLoading(true);
    storesApi
      .getAllMarketProducts()
      .then((res) => {
        const _products = res as ApiListData<Product>;
        setIsLoading(false);
        setError(null);
        setProducts(_products);
      })
      .catch((res) => {
        const error = res as ApiError;
        setIsLoading(false);
        setError(error.message ?? "Error.");
        setProducts([]);
        console.error("Error while fetching products: ", error);
      });
  }, []);

  useEffect(() => {
    getProducts();
  }, []);

  return {
    isLoading: isLoading,
    products: products,
    error: error
  };
};
