import { useCallback, useMemo } from "react";
import { useProducts } from "../../hooks/data/useProducts";
import { AppLoader } from "../../components/AppLoader/AppLoader";
import { NotFoundPage } from "../NotFoundPage/NotFoundPage";
import { Box, Divider, Grid, Paper, Typography, Button } from "@mui/material";
import { useCart } from "../../hooks/useCart";
import { CardProductEntry } from "./CartProductEntry";
import { Product } from "../../types/appTypes";
import React from "react";
import { PriceText } from "../../components/AppCarousel/AppProductCard";
import { FlexSpacer } from "../../components/FlexSpacer";

import ShoppingCartCheckoutIcon from "@mui/icons-material/ShoppingCartCheckout";
import { CheckoutDialog } from "./Checkout/CheckoutDialog";
import NiceModal from "@ebay/nice-modal-react";

export const CartPage = () => {
  const productsData = useProducts();
  const cartData = useCart();

  const productMap: { [id: number]: Product } = {};
  productsData.products.forEach((p) => {
    productMap[p.id] = p;
  });

  const totalPrice = cartData.cartItems.reduce(
    (sum, item) =>
      sum + item.quantity * (productMap[item.productId]?.price ?? 0),
    0
  );

  const handleCheckout = useCallback(() => {
    NiceModal.show(CheckoutDialog);
  }, []);

  if (productsData.isLoading) return <AppLoader />;

  return (
    <>
      <Grid container spacing={2}>
        {productsData.error && (
          <Grid xs={12} item>
            <Typography variant="h5" color="error">
              Error: {productsData.error}
            </Typography>
          </Grid>
        )}
        {/* Map Items */}
        {cartData.cartItems.length === 0 ? (
          <Grid xs={12} item>
            <Typography variant="h4">
              Looks like your cart is empty...
            </Typography>
          </Grid>
        ) : (
          <>
            <Grid xs={12} item>
              <Typography variant="h4">Cart Items: </Typography>
            </Grid>
            <Grid xs={12} item>
              <Typography
                variant="subtitle1"
                sx={(theme) => ({ color: theme.palette.warning.main })}
              >
                Warning! You may want to login to save your cart.{" "}
              </Typography>
            </Grid>
            {cartData.cartItems.map((item, idx) => {
              const p = productMap[item.productId];
              if (!p) return <React.Fragment key={idx}></React.Fragment>;
              return (
                <Grid key={idx} xs={12} item>
                  <Paper variant="outlined">
                    <CardProductEntry product={p} quantity={item.quantity} />
                  </Paper>
                </Grid>
              );
            })}
            <Grid xs={12} item>
              <Paper variant="outlined" sx={{ p: 2 }}>
                <Box sx={{ display: "flex" }}>
                  <Typography variant="h5">
                    Total Price (before discounts):{" "}
                  </Typography>
                  <FlexSpacer />
                  <PriceText price={totalPrice} />
                </Box>
                <Button
                  onClick={handleCheckout}
                  startIcon={<ShoppingCartCheckoutIcon />}
                  fullWidth
                  variant="contained"
                  color="secondary"
                  sx={{ mt: 2 }}
                >
                  Checkout
                </Button>
              </Paper>
            </Grid>
          </>
        )}
      </Grid>
    </>
  );
};
