import React, { useState, useCallback, useMemo } from 'react';

import { Card, CardMedia, CardContent, CardActions, Typography, Button, Box, Divider, Paper, Link } from '@mui/material';

import { Store } from '../../types/appTypes';
import { FlexSpacer } from '../FlexSpacer';
import { useNavigate } from 'react-router-dom';

interface AppStoreCardProps {
    store: Store,
    isSeller?: boolean
}

export const AppStoreCard = ({ store, isSeller = false }: AppStoreCardProps) => {
    const navigate = useNavigate();

    const storeUrl = useMemo(() => (
        isSeller
            ? `/seller/stores/${encodeURIComponent(store.id)}`
            : `/stores/${encodeURIComponent(store.id)}`
    ), [isSeller, store.id]);

        const handleClickManageStore = useCallback(() => {
            navigate(decodeURIComponent(storeUrl));
        }, [navigate, storeUrl]);

    return (
        <Box sx={(theme) => ({
            display: 'flex',
            width: '100%',
        })}>
            <Box sx={(theme) => ({
                width: '100%',
                display: 'flex',
                alignItems: 'center',
                flexDirection: 'column',
                p: 2
                // [theme.breakpoints.up('xs')]: {
                //     flexDirection: 'row',
                // },
                // [theme.breakpoints.down('sm')]: {
                //     flexDirection: 'column',
                // },
            })}>
                <Box sx={{
                    width: '100%',
                    display: 'flex',
                    alignItems: 'start',
                    flexDirection: 'column',
                }}>
                    <Box sx={{ display: 'flex', flexDirection: 'row', width: '100%', alignItems: 'end', textAlign: 'start' }}>
                        <Typography gutterBottom variant="h5">
                            <Link underline="none" href={storeUrl}>
                                {store.name}
                            </Link>
                        </Typography>
                        {/* <FlexSpacer />
                        <Typography gutterBottom variant="caption" sx={{ verticalAlign: 'bottom', display: 'block', mb: '11px' }} >
                            {store.description}
                        </Typography> */}
                    </Box>
                    <Divider orientation="horizontal" sx={{ width: '100%', mb: 1 }} />
                    <Typography variant="body2" color="text.secondary" sx={{ textAlign: 'justify' }} >
                        {store.description}
                    </Typography>
                </Box>
                <FlexSpacer sx={{ minHeight: 50 }} /> 
                <Box sx={{ display: 'flex', flexDirection: 'row', width: '100%' }}>
                    {/* <PriceText price={product.price} /> */}
                    <FlexSpacer />
                    <Button variant="contained" size="small" color="secondary" onClick={handleClickManageStore}>Manage Store</Button>
                </Box>
            </Box>
        </Box>
    );
}