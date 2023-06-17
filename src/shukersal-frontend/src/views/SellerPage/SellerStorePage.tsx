import { Navigate, useNavigate, useParams } from "react-router-dom";
import { useSellerStore } from "../../hooks/data/useSellerStore";
import { AppLoader } from "../../components/AppLoader/AppLoader";

import {
  DataGrid,
  GridRowsProp,
  GridColDef,
  gridColumnGroupsLookupSelector,
  GridCellParams,
} from "@mui/x-data-grid";
import {
  Button,
  Grid,
  IconButton,
  Paper,
  Stack,
  useMediaQuery,
  useTheme,
} from "@mui/material";

import AddIcon from "@mui/icons-material/Add";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from "@mui/icons-material/Delete";

import PercentIcon from "@mui/icons-material/Percent";
import ManageAccountsIcon from "@mui/icons-material/ManageAccounts";

import CheckIcon from "@mui/icons-material/Check";

import { FlexSpacer } from "../../components/FlexSpacer";
import { useCallback } from "react";
import NiceModal from "@ebay/nice-modal-react";
import { AppAreYouSureDialog } from "../../components/AppAreYouSureDialog";
import { NotFoundPage } from "../NotFoundPage/NotFoundPage";
import { AddProductDialog } from "./AddProduct/AddProductDialog";
import { APP_CATEGORIES } from "../../constants";
import { storeProductsApi } from "../../api/storesApi";
import { EditProductDialog } from './EditProduct/EditProductDialog';

const handleEditProduct = async (params: GridCellParams) => {
  const productId = params.row.id;
  const storeId = params.row.storeId;

  NiceModal.show(EditProductDialog, { initProduct: params.row, productId: productId, storeId: storeId }).then(() => {
    window.location.reload();
  });
};

const handleDeleteProduct = async (params: GridCellParams) => {
  const productId = params.row.id;
  const storeId = params.row.storeId;

  NiceModal.show(AppAreYouSureDialog, {
    bodyText: "This will delete the product permanently.",
    onYes: async () => {
      await storeProductsApi.delete(storeId, productId);
      window.location.reload(); // why use react when you can make the project way faster and way worst (to match the level of the be)
    },
  });
};

const columns: GridColDef[] = [
  {
    field: "isListed",
    headerName: "Listed",
    width: 30,
    renderCell: (params) => (params.value ? <CheckIcon /> : <></>),
  },
  { field: "name", headerName: "Name", width: 100 },
  {
    field: "category",
    headerName: "Category",
    width: 100,
    valueGetter: (params) => params.value?.name ?? "",
  },
  //{ field: "description", headerName: "Description", width: 200},
  { field: "price", headerName: "Price", width: 50 },
  { field: "unitsInStock", headerName: "Stock", width: 50 },
  {
    field: "imageUrl",
    headerName: "Image URL",
    flex: 1, // Stretch to fill available width
  },
  {
    field: "EDIT_BUTTON",
    headerName: "",
    width: 50,
    renderCell: (params) => (
      <IconButton onClick={() => handleEditProduct(params)}>
        <EditIcon />
      </IconButton>
    ),
  },
  {
    field: "DELETE_BUTTON",
    headerName: "",
    width: 50,
    renderCell: (params) => (
      <IconButton onClick={() => handleDeleteProduct(params)}>
        <DeleteIcon />
      </IconButton>
    ),
  },
];


export const SellerStorePage = () => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
  const navigate = useNavigate();

  const { storeId } = useParams();
  const fixedStoreId = Number(decodeURIComponent(storeId ?? "0"));
  const sellerStoreData = useSellerStore(fixedStoreId);

  const isLoading = sellerStoreData.isLoading;

  const handleAddProduct = useCallback(() => {
    const fixedStoreId = Number(storeId);
    if (fixedStoreId) {
      NiceModal.show(AddProductDialog, { storeId: fixedStoreId }).then(() => {
        sellerStoreData.getStoreProducts();
      });
    }
  }, [sellerStoreData, storeId]);

  const handleDeleteStore = useCallback(() => {
    NiceModal.show(AppAreYouSureDialog, {
      bodyText: "This will delete your store permanently. ",
      onYes: async () => {
        await sellerStoreData.deleteStore();
        navigate("/seller");
      },
    });
  }, [sellerStoreData.deleteStore]);

  if (isLoading) {
    return <AppLoader />;
  }

  if (!sellerStoreData.store) {
    // Invalid store
    return <NotFoundPage />;
  }

  return (
    <Grid container spacing={2}>
      <Grid xs={12} item>
        <Stack spacing={2} direction={isMobile ? "column" : "row"}>
          <Button
            fullWidth={isMobile}
            startIcon={<AddIcon />}
            variant="contained"
            onClick={handleAddProduct}
          >
            Add Product
          </Button>
          <FlexSpacer />
          <Button
            fullWidth={isMobile}
            startIcon={<DeleteIcon />}
            variant="contained"
            color="error"
            onClick={handleDeleteStore}
          >
            Delete Store
          </Button>
        </Stack>
      </Grid>
      <Grid xs={12} item>
        <Paper sx={{ height: 300, width: "100%" }}>
          <DataGrid rows={sellerStoreData.products} columns={columns} />
        </Paper>
      </Grid>
      <Grid xs={isMobile ? 12 : 6} item>
        <Button
          fullWidth
          startIcon={<ManageAccountsIcon />}
          variant="contained"
          onClick={() => {
            navigate("managers");
          }}
        >
          Managers
        </Button>
      </Grid>
      <Grid xs={isMobile ? 12 : 6} item>
        <Button
          fullWidth
          startIcon={<PercentIcon />}
          variant="contained"
          onClick={() => {
            navigate("discounts");
          }}
        >
          Discounts
        </Button>
      </Grid>
    </Grid>
  );
};
