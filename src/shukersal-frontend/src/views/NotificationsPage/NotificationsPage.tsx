import { Link, useParams } from "react-router-dom";
import { useStore } from "../../hooks/data/useStore";
import { useProducts } from "../../hooks/data/useProducts";
import { useCallback, useEffect, useMemo } from "react";
import { NotFoundPage } from "../NotFoundPage/NotFoundPage";
import {
  Box,
  Button,
  Divider,
  Grid,
  IconButton,
  Paper,
  Typography,
} from "@mui/material";
import { AppLoader } from "../../components/AppLoader/AppLoader";
import { AppProductCard } from "../../components/AppCarousel/AppProductCard";
import { DataGrid, GridCellParams, GridColDef } from "@mui/x-data-grid";
import { useNotifications } from "./useNotifications";
import { ntTypeData } from "../../types/appTypes";
import dayjs from "dayjs";
import { DISPLAY_DATE_TIME_FORMAT } from "../../_configuration";

import DeleteIcon from "@mui/icons-material/Delete";
import { notificationsApi } from "../../api/notificationsApi";
import { AppAreYouSureDialog } from "../../components/AppAreYouSureDialog";
import NiceModal from "@ebay/nice-modal-react";
import { FlexSpacer } from "../../components/FlexSpacer";
//   id: number;
//   message: string;
//   memberId: number;
//   notificationType: NotificationType;
//   createdAt: string;

const handleDeleteNt = async (params: GridCellParams) => {
  const ntId = params.row.id;

  await notificationsApi.delete(ntId);
  window.location.reload();
};

const columns: GridColDef[] = [
  {
    field: "ICON",
    headerName: "",
    width: 50,
    renderCell: (params) => ntTypeData[params.row.notificationType].icon,
  },
  {
    field: "Title",
    headerName: "Title",
    width: 200,
    renderCell: (params) => ntTypeData[params.row.notificationType].title,
  },
  {
    field: "message",
    headerName: "Message",
    flex: 1, // Stretch to fill available width
  },
  {
    field: "createdAt",
    headerName: "Date Time",
    width: 170,
    valueFormatter: (params) =>
      dayjs(params.value).format(DISPLAY_DATE_TIME_FORMAT),
  },

  {
    field: "DELETE_BUTTON",
    headerName: "",
    width: 50,
    renderCell: (params) => (
      <IconButton onClick={() => handleDeleteNt(params)}>
        <DeleteIcon />
      </IconButton>
    ),
  },
];

export const NotificationsPage = () => {
  const ntsData = useNotifications();

  useEffect(() => {
    ntsData.getNotifications();
  }, []);

  const handleDeleteAllNts = useCallback(() => {
    NiceModal.show(AppAreYouSureDialog, {
      bodyText: "This will delete ALL your notifications. ",
      onYes: async () => {
        await ntsData.deleteAllNotifications();
        // window.location.reload();
      },
    });
  }, [ntsData.deleteAllNotifications]);

  return (
    <>
      <Grid container spacing={2}>
        <Grid xs={12} item>
          <Box sx={{ display: "flex", width: "100%", alignItems: "center" }}>
            <Typography variant="h3">My Notifications</Typography>
            <FlexSpacer />
            <Box>
              <Button
                startIcon={<DeleteIcon />}
                variant="contained"
                color="error"
                onClick={handleDeleteAllNts}
              >
                Delete All
              </Button>
            </Box>
          </Box>
        </Grid>
        {/* Store Info */}
        <Grid xs={12} item>
          <Paper sx={{ height: 300, width: "100%" }}>
            <DataGrid rows={ntsData.nts} columns={columns} />
          </Paper>
        </Grid>
      </Grid>
    </>
  );
};
