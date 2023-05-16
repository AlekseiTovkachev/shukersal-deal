import { useParams } from 'react-router-dom';
import { useSellerStore } from '../../hooks/data/useSellerStore';
import { AppLoader } from '../../components/AppLoader/AppLoader';

import { DataGrid, GridRowsProp, GridColDef } from '@mui/x-data-grid';
import { Button, Grid, Paper, useMediaQuery, useTheme } from '@mui/material';

import AddIcon from '@mui/icons-material/Add';

const rows: GridRowsProp = [
    { id: 1, col1: 'Hello', col2: 'World' },
    { id: 2, col1: 'DataGridPro', col2: 'is Awesome' },
    { id: 3, col1: 'MUI', col2: 'is Amazing' },
];

const columns: GridColDef[] = [
    { field: 'col1', headerName: 'Column 1', width: 150 },
    { field: 'col2', headerName: 'Column 2', width: 150 },
];

export const SellerStorePage = () => {
    // TODO: Implement
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('sm'));
    const { storeId } = useParams();
    const fixedStoreId = Number(decodeURIComponent(storeId ?? '0'));
    const sellerStoreData = useSellerStore(fixedStoreId);

    const isLoading = sellerStoreData.isLoading;

    if (isLoading) {
        return <AppLoader />;
    }

    return <Grid container spacing={2}>
        <Grid xs={12} sm={6} md={4} lg={3} item>
            <Button
                fullWidth={isMobile}
                startIcon={<AddIcon />}
                variant="contained">
                Add Product
            </Button>
        </Grid>
        <Grid xs={12} item>
            <Paper sx={{ height: 300, width: '100%' }}>
                <DataGrid rows={rows} columns={columns} />
            </Paper>
        </Grid>
    </Grid>
}