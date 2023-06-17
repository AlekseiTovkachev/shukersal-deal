import { useParams } from "react-router-dom";

import { AppLoader } from "../../../components/AppLoader/AppLoader";
import { NotFoundPage } from "../../NotFoundPage/NotFoundPage";
import { useSellerStore } from "../../../hooks/data/useSellerStore";
import { useManagers } from "./useManagers";
import { TreeView } from "@mui/lab";

import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import { Paper, Grid, Button, Typography } from "@mui/material";
import { makeTree } from "./util";

import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import AdminPanelSettingsIcon from "@mui/icons-material/AdminPanelSettings";
import { useIsMobile } from "../../../hooks/mediaHooks";
import { useAuth } from "../../../hooks/useAuth";
import { useCallback, useMemo } from "react";
import { StoreManager } from "../../../types/appTypes";
import { AddManagerDialog } from "./AddManagerDialog";
import NiceModal from "@ebay/nice-modal-react";
import { getStore } from "../../../redux/storeSlice";
import { useAppDispatch } from "../../../hooks/useAppDispatch";

export const SellerStoreManagersPage = () => {
  const isMobile = useIsMobile();
  const dispatch = useAppDispatch();

  const { storeId } = useParams();
  const fixedStoreId = Number(decodeURIComponent(storeId ?? "0"));
  const sellerStoreData = useSellerStore(fixedStoreId);
  const managersData = useManagers(fixedStoreId);

  const authData = useAuth();
  const loggedManagerId = useMemo(() => {
    if (!managersData.rootManager) return null;
    const memberId = authData.currentMemberData?.currentMember.id;
    function f(m: StoreManager) {
      if (m.memberId === memberId) return m.id;
      let returnValue = null;
      m.childManagers.forEach((c) => {
        const res = f(c);
        if (res) {
          returnValue = res;
        }
      });
      return returnValue;
    }
    return f(managersData.rootManager);
  }, [authData.currentMemberData, managersData.rootManager]);

  const isLoading = sellerStoreData.isLoading || managersData.isLoading;

  const handleAddManager = useCallback((asOwner: boolean) => {
    if (loggedManagerId)
      NiceModal.show(AddManagerDialog, {
        storeId: fixedStoreId,
        bossId: loggedManagerId,
        asOwner: asOwner
      }).then(() => {
        window.location.reload(); // getto af
        dispatch(getStore(fixedStoreId));
      });
  }, [loggedManagerId, fixedStoreId]);

  if (isLoading) {
    return <AppLoader />;
  }

  if (!sellerStoreData.store || !managersData.rootManager || !loggedManagerId) {
    // Invalid store
    return <NotFoundPage />;
  }

  return (
    <>
      <Grid
        container
        spacing={2}
        sx={{
          width: "100%",
        }}
      >
        <Grid item xs={isMobile ? 12 : 6}>
          <Button
            onClick={() => {
              handleAddManager(false);
            }}
            fullWidth={isMobile}
            variant="contained"
            startIcon={<AccountCircleIcon />}
          >
            Add Manager
          </Button>
        </Grid>
        <Grid item xs={isMobile ? 12 : 6}>
          <Button
            onClick={() => {
              handleAddManager(true);
            }}
            fullWidth={isMobile}
            variant="contained"
            startIcon={<AdminPanelSettingsIcon />}
          >
            Add Owner
          </Button>
        </Grid>

        <Grid item xs={12}>
          <Typography variant="h5">Store Managers:</Typography>
        </Grid>

        <Grid item xs={12}>
          <Paper sx={{ width: "100%", minHeight: 200 }} variant="outlined">
            <TreeView
              aria-label="file system navigator"
              defaultCollapseIcon={<ExpandMoreIcon />}
              defaultExpandIcon={<ChevronRightIcon />}
              sx={{ height: "100%", p: 1, mr: 2 }}
            >
              {makeTree(loggedManagerId, managersData.rootManager)}
            </TreeView>
          </Paper>
        </Grid>
      </Grid>
    </>
  );
};