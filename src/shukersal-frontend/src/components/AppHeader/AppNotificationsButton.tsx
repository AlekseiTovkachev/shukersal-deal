import React, { useCallback, useEffect } from "react";
import { useNavigate } from "react-router-dom";

import Badge, { BadgeProps } from "@mui/material/Badge";
import { styled } from "@mui/material/styles";
import IconButton from "@mui/material/IconButton";
import NotificationsIcon from "@mui/icons-material/Notifications";
import { useCart } from "../../hooks/useCart";
import { useNotifications } from "../../views/NotificationsPage/useNotifications";

const StyledBadge = styled(Badge)<BadgeProps>(({ theme }) => ({
  "& .MuiBadge-badge": {
    right: -3,
    top: 18,
    // border: `2px solid ${theme.palette.background.paper}`,
    padding: "0 0px",
  },
}));

export const AppNotificationsButton = () => {
  const navigate = useNavigate();

  const ntsData = useNotifications();

  const handleClick = useCallback(() => {
    navigate("/notifications");
  }, [navigate]);

  useEffect(() => {
    ntsData.getNotifications();
  }, []);
  return (
    <IconButton color="secondary" onClick={handleClick}>
      {ntsData.nts.length > 0 ? (
        <StyledBadge badgeContent={ntsData.nts.length} color="info">
          <NotificationsIcon />
        </StyledBadge>
      ) : (
        <NotificationsIcon />
      )}
    </IconButton>
  );
};
