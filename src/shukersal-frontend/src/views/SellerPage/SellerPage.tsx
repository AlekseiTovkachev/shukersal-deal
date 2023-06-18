import { Navigate, useNavigate } from "react-router-dom";

import NiceModal from "@ebay/nice-modal-react";

import AddIcon from "@mui/icons-material/Add";

import { useAuth } from "../../hooks/useAuth";
import { useSeller } from "../../hooks/data/useSeller";
import { AppLoader } from "../../components/AppLoader/AppLoader";
import { AppNoStoresPropmt } from "../../components/AppNoStoresPropmt";
import {
  Button,
  Box,
  Grid,
  Paper,
  Typography,
  useTheme,
  useMediaQuery,
} from "@mui/material";
import { AppStoreCard } from "../../components/AppCarousel/AppStoreCard";
import { useCallback, useEffect } from "react";
import { OpenNewStoreDialog } from "./OpenNewStore/OpenNewStoreDialog";

export const SellerPage = () => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

  const sellerData = useSeller();
  const authData = useAuth();

  const isLoading = sellerData.isLoading || authData.isLoading;

  const handleOpenNewStore = useCallback(() => {
    NiceModal.show(OpenNewStoreDialog);
  }, []);

  if (isLoading) {
    return <AppLoader />;
  }

  if (!authData.isLoggedIn) {
    return <Navigate to="login" />;
  }

  return (
    <>
      <Grid container spacing={2}>
        <Grid xs={12} sm={6} md={4} lg={3} item>
          <Button
            fullWidth={isMobile}
            startIcon={<AddIcon />}
            variant="contained"
            onClick={() => {
              handleOpenNewStore();
            }}
          >
            Open New Store
          </Button>
        </Grid>
        <Grid xs={12} item>
          <Typography variant="h3">My Stores</Typography>
        </Grid>
        {sellerData.stores?.length < 1 ? (
          <Grid xs={12} item>
            <AppNoStoresPropmt />
          </Grid>
        ) : (
          sellerData.stores.map((store, index) => (
            <Grid key={index} xs={12} item>
              <Paper
                variant="outlined"
                sx={(theme) => ({
                  width: "100%",
                })}
              >
                <AppStoreCard store={store} isSeller={true} />
              </Paper>
            </Grid>
          ))
        )}
      </Grid>
    </>
  );
};
