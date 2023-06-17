import { useParams } from "react-router-dom";
import { useStore } from "../../hooks/data/useStore";
import { AppLoader } from "../../components/AppLoader/AppLoader";
import { Divider, Grid, Paper, Typography } from "@mui/material";
import { AppProductCard } from "../../components/AppCarousel/AppProductCard";
import { NotFoundPage } from "../NotFoundPage/NotFoundPage";
import { useEffect } from "react";

export const StorePage = () => {
  const { storeId } = useParams();
  const fixedStoreId = Number(decodeURIComponent(storeId ?? "0"));
  const { getStore, getStoreProducts, store, storeProducts, isLoading, error } = useStore(fixedStoreId);

  useEffect(() => {
    getStore();
    getStoreProducts();
  }, []);

  if (isLoading) {
    return <AppLoader />;
  }

  if (!store) {
    return <NotFoundPage />;
  }

  return (
    <>
      <Grid container spacing={2}>
        <Grid xs={12} item>
          {error && (
            <Typography variant="h5" color="error">
              Error: {error}
            </Typography>
          )}
        </Grid>
        <Grid xs={12} item>
          <Typography variant="h2">{store.name}</Typography>
        </Grid>
        <Grid xs={12} item>
          <Typography variant="body1">{store.description}</Typography>
        </Grid>
        <Grid xs={12} item>
          <Divider />
        </Grid>
        <Grid xs={12} item>
          <Typography variant="h4">Products</Typography>
        </Grid>
        {storeProducts.map((p, idx) => (
          <Grid key={idx} xs={12} item>
            <Paper variant="outlined">
              <AppProductCard product={p} />
            </Paper>
          </Grid>
        ))}
      </Grid>
    </>
  );
};
