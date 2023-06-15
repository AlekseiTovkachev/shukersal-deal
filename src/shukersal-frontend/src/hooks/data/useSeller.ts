import { SELLER_ID_1 } from "./DEMO_DATA_useSeller";
import { useCallback, useEffect, useState } from "react";
import { demoStores } from "./DEMO_DATA_useStores";
import { useAppDispatch } from "../useAppDispatch";
import { useAppSelector } from "../useAppSelector";
import { StorePostFormFields } from "../../types/formTypes";
import { createStore, getAllStore, getMyStores } from "../../redux/storeSlice";
import { useAuth } from "../useAuth";


export const useSeller = (isUsingStores = true) => {
  const dispatch = useAppDispatch();
  
  const authData = useAuth();

  const myStores = useAppSelector((state) => state.store.myStores);
  const isLoadingStoreService = useAppSelector(
    (state) => state.store.isLoading
  );
  const storeServiceError = useAppSelector((state) => state.store.error);

  const isLoading = isLoadingStoreService; // || isLoadingManagerService

  const getMyStoresCallback = useCallback(async () => {
    if (isUsingStores) {
      if (authData.currentMemberData?.currentMember?.id)
        dispatch(getMyStores(authData.currentMemberData.currentMember.id));
    }
  }, [dispatch, authData, isUsingStores]);

  const createStoreCallback = useCallback(
    async (formData: StorePostFormFields) => {
      const response = await dispatch(createStore(formData));
      if (response.meta.requestStatus === "fulfilled") {
        return true;
      }
      return false;
    },
    [dispatch]
  );

  useEffect(() => {
    getMyStoresCallback();
  }, []);

  return {
    // sellerIds: [SELLER_ID_1],
    isLoading: isLoading,
    error: storeServiceError, // || managerServiceError
    stores: myStores,

    refreshMyStores: getMyStoresCallback,
    createStore: createStoreCallback,
  };
};
