import { Box, Button, Link, Typography, Divider } from '@mui/material';
import { Product } from "../../types/appTypes";
import { FlexSpacer } from "../../components/FlexSpacer";
import { useCallback, useMemo, useState } from "react";
import { useCart } from "../../hooks/useCart";
import { PriceText } from "../../components/AppCarousel/AppProductCard";
import { AppQtyCounter } from "../../components/AppCarousel/AppQtyCounter";

import DeleteIcon from "@mui/icons-material/Delete";

export const CardProductEntry = ({
  product,
  quantity,
}: {
  product: Product;
  quantity: number;
}) => {
  const productUrl = useMemo(
    () => `/products/${encodeURIComponent(product.id)}`,
    [product.id]
  );

  const cartData = useCart();

  const deleteCartItem = useCallback(() => {
    cartData.deleteItem({
      productId: product.id,
      quantity: quantity,
    });
  }, [cartData, product]);

  const updateCartItemQuantity = useCallback(
    (newQuantity: number) => {
      if (newQuantity === 0) {
        deleteCartItem();
      } else {
        cartData.updateItem({
          productId: product.id,
          quantity: newQuantity,
        });
      }
    },
    [cartData, product, deleteCartItem]
  );

  return (
    <Box display="flex" /* sx={{ boxSizing: 'border-box', p: 1}} */>
      <Box
        sx={(theme) => ({
          display: "flex",
          width: "100%",
        })}
      >
        <Box
          sx={(theme) => ({
            backgroundImage: `url("${product.imageUrl}")`,
            backgroundSize: "cover",
            backgroundRepeat: "no-repeat",
            backgroundPosition: "center",
            width: "30%",
          })}
        >
          <Link
            href={productUrl}
            sx={{
              display: "block",
              width: "100%",
              height: "100%",
              textDecotraion: "none",
            }}
          ></Link>
        </Box>
        <Divider orientation='vertical' sx={{mr: 1}}/>
        <Box
          sx={(theme) => ({
            display: "flex",
            alignItems: "star",
            flexDirection: "column",
            p: 2,
            // [theme.breakpoints.up('xs')]: {
            //     flexDirection: 'row',
            // },
            // [theme.breakpoints.down('sm')]: {
            //     flexDirection: 'column',
            // },
          })}
        >
          <Typography gutterBottom variant="h5">
            <Link underline="none" href={productUrl}>
              {product.name}
            </Link>
          </Typography>
          <FlexSpacer />
          <Typography
            gutterBottom
            variant="caption"
            sx={{ verticalAlign: "bottom", display: "block", mb: "11px" }}
          >
            {product.category.name}
          </Typography>
        </Box>
        <FlexSpacer />
        <Box
          sx={{
            display: "flex",
            flexDirectino: "center",
            justifyContent: "center",
            alignItems: "center",
            height: "100%",
          }}
        >
          <PriceText price={product.price} />
        </Box>
        <AppQtyCounter
          initValue={quantity}
          minVal={0}
          onChange={(newValue) => {
            updateCartItemQuantity(newValue);
          }}
        />
        <Box
          sx={{
            display: "flex",
            flexDirectino: "center",
            justifyContent: "center",
            alignItems: "center",
            height: "100%",
            mr: 4
          }}
        >
          <Button
            onClick={deleteCartItem}
            variant="contained"
            size="small"
            color="error"
            startIcon={<DeleteIcon />}
            //   disabled={cartData.isLoading}
          >
            Remove
          </Button>
        </Box>
      </Box>
    </Box>
  );
};
