import { useCallback, useEffect, useState } from "react";
import { TransactionPostData, cartApi } from "../../../api/cartApi";
import { Transaction } from "../../../types/appTypes";
import { ApiError } from "../../../types/apiTypes";

export const useCheckout = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const submitTransaction = useCallback(
    async (transactionPostData: TransactionPostData) => {
      setIsLoading(true);
      return cartApi
        .submitTransaction(transactionPostData)
        .then((res) => {
          const transaction = res as Transaction;
          setIsLoading(false);
          setError(null);
          return Promise.resolve(true);
        })
        .catch((res) => {
          const error = res as ApiError;
          setIsLoading(false);
          let errmsg = error.message ?? "Error.";
          if (typeof errmsg !== "string") errmsg = JSON.stringify(errmsg);
          setError(errmsg);
          console.error("Error while fetching products: ", error);
          return Promise.resolve(false);
        });
    },
    []
  );

  return {
    isLoading: isLoading,
    submitTransaction: submitTransaction,
    error: error,
  };
};
