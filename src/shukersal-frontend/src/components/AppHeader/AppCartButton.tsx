import React, { useCallback } from "react";
import { useNavigate } from "react-router-dom";

import Badge, { BadgeProps } from "@mui/material/Badge";
import { styled } from "@mui/material/styles";
import IconButton from "@mui/material/IconButton";
import ShoppingCartIcon from "@mui/icons-material/ShoppingCart";
import { useCart } from "../../hooks/useCart";

const StyledBadge = styled(Badge)<BadgeProps>(({ theme }) => ({
  "& .MuiBadge-badge": {
    right: -3,
    top: 18,
    // border: `2px solid ${theme.palette.background.paper}`,
    padding: "0 0px",
  },
}));

export const AppCartButton = () => {
  const navigate = useNavigate();

  const cartData = useCart();

  const handleClick = useCallback(() => {
    navigate("/cart");
  }, [navigate]);

  return (
    <IconButton color="secondary" onClick={handleClick}>
      {cartData.cartItems.length > 0 ? (
        <StyledBadge badgeContent={cartData.cartItems.length} color="info">
          <ShoppingCartIcon />
        </StyledBadge>
      ) : (
        <ShoppingCartIcon />
      )}
    </IconButton>
  );
};
