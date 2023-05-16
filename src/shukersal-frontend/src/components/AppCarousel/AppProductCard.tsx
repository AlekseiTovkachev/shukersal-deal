import React, { useState, useCallback, useMemo } from 'react';

import { Card, CardMedia, CardContent, CardActions, Typography, Button, Box, Divider, Paper, Link } from '@mui/material';

import { Product } from '../../types/appTypes';
import { FlexSpacer } from '../FlexSpacer';
import { APP_CURRENCY_SIGN } from '../../_configuration';

const PriceText = ({ price }: { price: number }) => {
    const integerPart = Math.floor(price).toString();
    const decimalPart = (price % 1).toFixed(2).substring(2);
    return <Box sx={{
        boxSizing: 'border-box'
    }}>
        <Typography component="span" variant="h5" color=""
            sx={{
                display: 'inline-block',
                verticalAlign: 'top'
            }}>
            {APP_CURRENCY_SIGN + integerPart}
        </Typography>
        <Typography component="span" variant="caption" color="text.secondary"
            sx={{
                display: 'inline-block',
                verticalAlign: 'top',
                mt: '2px'
            }}>
            {decimalPart}
        </Typography>
    </Box>
}

interface AppProductCardProps {
    product: Product
}

export const AppProductCard = ({ product }: AppProductCardProps) => {
    const productUrl = useMemo(() => (`/products/${encodeURIComponent(product.id)}`), [product.id]);

    return (
        <Box sx={(theme) => ({
            display: 'flex',
            width: '100%',

            // [theme.breakpoints.up('xs')]: {
            //     flexDirection: 'row',
            // },
            [theme.breakpoints.down('sm')]: {
                flexDirection: 'column',
            },
        })}>
            <Box sx={(theme) => ({
                [theme.breakpoints.up('xs')]: {
                    width: '100%',
                    height: 300,
                },
                [theme.breakpoints.up('sm')]: {
                    width: '100%',
                    height: 300,
                },
                maxWidth: 500,
            })}>
                <Box
                    // component="img"
                    // src={product.imageUrl}

                    sx={(theme) => ({
                        backgroundImage: `url("${product.imageUrl}")`,
                        backgroundSize: 'cover',
                        backgroundRepeat: 'no-repeat',
                        backgroundPosition: 'center',
                        height: '100%',
                        width: '100%',
                    })}
                >
                    <Link href={productUrl} sx={{ display: 'block', width: '100%', height: '100%', textDecotraion: 'none' }}></Link>
                </Box>
            </Box>
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
                            <Link underline="none" href={productUrl}>
                                {product.name}
                            </Link>
                        </Typography>
                        <FlexSpacer />
                        <Typography gutterBottom variant="caption" sx={{ verticalAlign: 'bottom', display: 'block', mb: '11px' }} >
                            {product.category.name}
                        </Typography>
                    </Box>
                    <Divider orientation="horizontal" sx={{ width: '100%', mb: 1 }} />
                    <Typography variant="body2" color="text.secondary" sx={{ textAlign: 'justify' }} >
                        {product.description}
                    </Typography>
                </Box>
                <FlexSpacer />
                <Box sx={{ display: 'flex', flexDirection: 'row', width: '100%' }}>
                    <PriceText price={product.price} />
                    <FlexSpacer />
                    <Button variant="contained" size="small" color="secondary">Add to Cart</Button>
                </Box>
            </Box>
        </Box>
    );
}