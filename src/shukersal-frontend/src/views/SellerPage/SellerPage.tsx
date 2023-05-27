import { Navigate, useNavigate } from 'react-router-dom';

import AddIcon from '@mui/icons-material/Add';

import { useAuth } from '../../hooks/useAuth';
import { useSeller } from '../../hooks/data/useSeller';
import { AppLoader } from "../../components/AppLoader/AppLoader";
import { AppStartSellingPrompt } from '../../components/AppStartSellingPrompt';
import { Button, Box, Grid, Paper, Typography, useTheme, useMediaQuery } from '@mui/material';
import { AppStoreCard } from '../../components/AppCarousel/AppStoreCard';

export const SellerPage = () => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('sm'));

    const sellerData = useSeller();
    const authData = useAuth();

    const isLoading = sellerData.isLoading || authData.isLoading;

    if (isLoading) {
        return <AppLoader />
    }

    if (!authData.isLoggedIn) {
        return <Navigate to='login' />
    }

    if (sellerData.sellerIds.length < 1) {
        return <AppStartSellingPrompt />
    }

    return <>
        <Grid container spacing={2}>
            <Grid xs={12} sm={6} md={4} lg={3} item>
                <Button
                    fullWidth={isMobile}
                    startIcon={<AddIcon />}
                    variant="contained">
                    Open New Store
                </Button>
            </Grid>
            <Grid xs={12} item>
                <Typography variant="h3">My Stores</Typography>
            </Grid>
            {sellerData.stores.map((store, index) => (
                <Grid key={index} xs={12} item>
                    <Paper sx={theme => ({
                        width: '100%',
                    })}>
                        <AppStoreCard store={store} isSeller={true} />
                    </Paper>
                </Grid>
            ))}
        </Grid >
    </>
}