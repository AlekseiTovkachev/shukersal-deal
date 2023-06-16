import { useEffect, useState } from "react";
import { discountsApi } from "./discountsApi";
import { ApiError, ApiListData } from "../../../../types/apiTypes";
import { Typography } from '@mui/material';
import { AppLoader } from "../../../../components/AppLoader/AppLoader";

interface DiscountsLogicProps {
  storeId: number;
}

const DiscountsLogic = ({ storeId }: DiscountsLogicProps) => {
  const [discounts, setDiscounts] = useState<DiscountRule[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [apiError, setApiError] = useState<string | null>(null);

  useEffect(() => {
    setIsLoading(true);
    discountsApi
      .getAll(storeId)
      .then((res) => {
        const responseDiscounts = res as ApiListData<DiscountRule>;
        setIsLoading(false);
        setApiError(null);
        setDiscounts(responseDiscounts)
      })
      .catch((res) => {
        const error = res as ApiError;
        setIsLoading(false);
        setApiError(error.message ?? "Error.");
        setDiscounts([]);
      });
  }, []);

  if(isLoading) 
    return <AppLoader />

  return (
    <>
      <Typography variant="h3">Discounts Logic for store {storeId}!</Typography>
      <Typography variant="h4">Here are your discounts:</Typography>
      {discounts.map((discount) => (
        <Typography variant="body1">{JSON.stringify(discount)}</Typography>
      ))}
      {apiError && <Typography variant="h5" color="error">{apiError}</Typography>}
    </>
  );
};

export default DiscountsLogic;
