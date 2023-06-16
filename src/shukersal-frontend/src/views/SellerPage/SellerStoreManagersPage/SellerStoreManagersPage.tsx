import { useParams } from "react-router-dom";

import { AppLoader } from "../../../components/AppLoader/AppLoader";
import { NotFoundPage } from "../../NotFoundPage/NotFoundPage";
import { useSellerStore } from "../../../hooks/data/useSellerStore";

export const SellerStoreManagersPage  = () => {
  const { storeId } = useParams();
  const fixedStoreId = Number(decodeURIComponent(storeId ?? "0"));
  const sellerStoreData = useSellerStore(fixedStoreId);
  const isLoading = sellerStoreData.isLoading;

  if (isLoading) {
    return <AppLoader />;
  }

  if (!sellerStoreData.store) {
    // Invalid store
    return <NotFoundPage />;
  }

  return (
    <>
      SellerStoreManagersPage 
    </>
  );
};
