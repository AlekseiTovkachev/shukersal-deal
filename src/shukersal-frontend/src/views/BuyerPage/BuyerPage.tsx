import { Card, Grid, CardContent, Typography, Box, Paper } from "@mui/material";
import { useCallback, useState } from "react";
import { useNavigate } from "react-router-dom";

import _ from "lodash";

import { useMembers } from "../../hooks/data/useMembers";
import { useProducts } from "../../hooks/data/useProducts";

import { AppPageScrollableContainer } from "../../components/AppPageScrollableContainer";
import { AppLoader } from "../../components/AppLoader/AppLoader";
import { AppSearchBar } from "../../components/AppSearchBar";
import { AppCarousel } from "../../components/AppCarousel/AppCarousel";
import { AppProductCard } from "../../components/AppCarousel/AppProductCard";
import { AppStoreCard } from "../../components/AppCarousel/AppStoreCard";
import { useStores } from "../../hooks/data/useStores";
import { filterProduct } from "./util";

export const BuyerPage = () => {
  const navigate = useNavigate();

  const { members } = useMembers();
  const {
    products,
    isLoading: isLoadingProducts,
    error: productsError,
  } = useProducts();
  const {
    stores,
    isLoading: isLoadingStores,
    error: storesError,
  } = useStores();

  const isLoading = isLoadingProducts || isLoadingStores;

  const [searchText, setSearchText] = useState<string>("");

  if (isLoading) {
    return <AppLoader />;
  }

  return (
    <>
      <Grid container spacing={2}>
        <Grid xs={12} item>
          {productsError && (
            <Typography variant="h5" color="error">
              Error: {productsError}
            </Typography>
          )}
          {storesError && (
            <Typography variant="h5" color="error">
              Error: {storesError}
            </Typography>
          )}
        </Grid>
        {/* -------------------- Search Bar -------------------- */}
        {/* <Grid xs={0} sm={2} lg={3} item></Grid> <== NOT USED (For centering search bar) */}
        <Grid xs={12} item>
          <AppSearchBar
            onChange={(v) => {
              setSearchText(v);
            }}
          />
        </Grid>
        {/* -------------------- Search Bar -------------------- */}

        {/* -------------------- Sales and Auctions -------------------- */}
        {/* TODO: Implement */}
        {/* -------------------- Sales and Auctions -------------------- */}

        {/* -------------------- Products -------------------- */}
        {/* <Grid xs={12} item>
                <AppCarousel
                    slides={products.map((product, index) =>
                        <AppProductCard key={index} product={product} />
                    )}
                />
            </Grid> */}

        <Grid xs={12} item>
          <Typography variant="h3">Products</Typography>
        </Grid>
        {products
          .filter((p) => (searchText ? filterProduct(p, searchText) : true))
          .map((p, idx) => (
            <Grid key={idx} xs={12} item>
              <Paper variant="outlined">
                <AppProductCard product={p} />
              </Paper>
            </Grid>
          ))}

        {/* -------------------- Products Items -------------------- */}

        {/* -------------------- Stores -------------------- */}
        <Grid xs={12} item>
          <Typography variant="h3">Stores</Typography>
        </Grid>
        {/* TODO: Implement */}
        <Grid xs={12} item>
          <AppCarousel
            slides={stores.map((store, index) => (
              <AppStoreCard key={index} store={store} />
            ))}
          />
        </Grid>
        {/* -------------------- Stores -------------------- */}
      </Grid>
    </>
  );
};
