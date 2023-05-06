import { Card, Grid, CardContent, Typography, Box } from '@mui/material';
import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import _ from 'lodash';

import { useMembers } from '../../hooks/data/useMembers';
import { useProducts } from '../../hooks/data/useProducts';

import { AppPageScrollableContainer } from '../../components/AppPageScrollableContainer';
import { AppLoader } from '../../components/AppLoader/AppLoader';
import { AppSearchBar } from '../../components/AppSearchBar';
import { AppCarousel } from '../../components/AppCarousel/AppCarousel';
import { AppProductCard } from '../../components/AppCarousel/AppProductCard';

export const BuyerPage = () => {
    const { members } = useMembers();
    const { products } = useProducts();

    return <>
        <Grid container spacing={2}>
            { /* -------------------- Search Bar -------------------- */}
            { /* <Grid xs={0} sm={2} lg={3} item></Grid> <== NOT USED (For centering search bar) */}
            <Grid xs={12} item>
                <AppSearchBar />
            </Grid>
            { /* -------------------- Search Bar -------------------- */}



            { /* -------------------- Sales and Auctions -------------------- */}
            { /* TODO: Implement */}
            { /* -------------------- Sales and Auctions -------------------- */}

            { /* -------------------- Recommended Items -------------------- */}
            { /* TODO: Implement */}
            <Grid xs={12} item>
                <AppCarousel
                    slides={products.map((product, index) =>
                        <AppProductCard key={index} product={product} />
                    )}
                />
            </Grid>
            <Grid xs={12} item>
                <AppCarousel
                    slides={products.map((product, index) =>
                        <AppProductCard key={index} product={product} />
                    )}
                />
            </Grid>
            <Grid xs={12} item>
                <AppCarousel
                    slides={products.map((product, index) =>
                        <AppProductCard key={index} product={product} />
                    )}
                />
            </Grid>
            <Grid xs={12} item>
                <AppCarousel
                    slides={products.map((product, index) =>
                        <AppProductCard key={index} product={product} />
                    )}
                />
            </Grid>
            <Grid xs={12} item>
                <AppCarousel
                    slides={products.map((product, index) =>
                        <AppProductCard key={index} product={product} />
                    )}
                />
            </Grid>
            { /* -------------------- Recommended Items -------------------- */}

            { /* -------------------- Recommended Stores -------------------- */}
            { /* TODO: Implement */}
            { /* -------------------- Recommended Stores -------------------- */}
        </Grid>
    </>
}