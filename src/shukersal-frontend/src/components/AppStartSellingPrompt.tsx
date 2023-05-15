import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import { Box, Typography, Button } from '@mui/material';

export const  AppStartSellingPrompt = () => {
    const navigate = useNavigate();

    const handleClick = useCallback(() => {
        // TODO: Implement opening of a seller account
        // await sendToApi
        // navigate to seller for refresh
        navigate('seller');
    }, [navigate]);

    return <Box>
        <Typography variant="h3">Seems like you don't have a seller account...</Typography>
        <Button onClick={handleClick}>Start Selling</Button>
    </ Box>
}