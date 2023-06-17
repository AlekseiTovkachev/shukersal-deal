import { Link, useParams } from "react-router-dom";
import { useStore } from "../../hooks/data/useStore";
import { useProducts } from "../../hooks/data/useProducts";
import { useEffect, useMemo } from "react";
import { NotFoundPage } from "../NotFoundPage/NotFoundPage";
import { Button, Divider, Grid, Paper, Typography } from "@mui/material";
import { AppLoader } from "../../components/AppLoader/AppLoader";
import { AppProductCard } from "../../components/AppCarousel/AppProductCard";

export const ProductPage = () => {
  const { productId } = useParams();
  const fixedProductId = useMemo(() => Number(productId ?? 0), [productId]);
  const productsData = useProducts();

  const product = useMemo(
    () => productsData.products.find((p) => p.id === fixedProductId),
    [productsData, fixedProductId]
  );

  const storeData = useStore(product?.storeId ?? 0);

  const productStore = storeData.store;
  useEffect(() => {
    if (product?.storeId && product.storeId != 0) storeData.getStore();
  }, [product]);

  if (productsData.isLoading || storeData.isLoading) return <AppLoader />;

  if (!product || !productStore) return <NotFoundPage />;

  return (
    <>
      <Grid container spacing={2}>
        {/* Product Info */}
        <Grid item xs={12}>
          <Paper variant="outlined">
            <AppProductCard product={product} />
          </Paper>
        </Grid>
        {/* Store Info */}
        <Grid item xs={12}>
          <Paper variant="outlined" style={{ padding: "16px" }}>
            <Typography variant="h5" gutterBottom>
              Available at {productStore.name}
            </Typography>
            <Typography variant="body2">{productStore.description}</Typography>
            <Divider style={{ margin: "16px 0" }} />
            <Button
              variant="contained"
              color="primary"
              component={Link}
              to={`/stores/${productStore.id}`}
            >
              Visit Store
            </Button>
          </Paper>
        </Grid>
      </Grid>
    </>
  );
};
