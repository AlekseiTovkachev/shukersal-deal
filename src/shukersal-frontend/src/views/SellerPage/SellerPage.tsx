import { Navigate, useNavigate } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { useSeller } from '../../hooks/data/useSeller';
import { AppLoader } from "../../components/AppLoader/AppLoader";
import { AppStartSellingPrompt } from '../../components/AppStartSellingPrompt';
import { Button, Box, Grid, Paper, Typography } from '@mui/material';
import { AppStoreCard } from '../../components/AppCarousel/AppStoreCard';

export const SellerPage = () => {
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
            <Grid xs={3} item>
                <Button variant="contained">Open New Store</Button>
            </Grid>
            <Grid xs={12} item>
                <Typography variant="h3">My Stores</Typography>
            </Grid>
            {sellerData.stores.map((store, index) => (
                <Grid xs={12} item>
                    <Paper key={index} sx={theme => ({
                        width: '100%',
                    })}>
                        <AppStoreCard store={store} isSeller={true} />
                    </Paper>
                </Grid>
            ))}
        </Grid >
    </>
}