import { Box, IconButton } from "@mui/material/";

import ArrowBackIcon from "@mui/icons-material/ArrowBack";

import { useNavigate, useLocation } from "react-router-dom";

export const AppBackButtonBar = () => {
  const navigate = useNavigate();
  const location = useLocation();

  const isNotRoot = location.pathname !== "/";
  const isNotDefault = location.key !== "default";
  const shouldShowBackButton = isNotRoot && isNotDefault;

  const goBack = () => {
    if (isNotDefault) {
      navigate(-1);
    }
  };
  
  return (
    <Box sx={{ display: "flex", width: "100%" }}>
      {shouldShowBackButton && (
        <IconButton onClick={goBack}>
          <ArrowBackIcon />
        </IconButton>
      )}
    </Box>
  );
};
