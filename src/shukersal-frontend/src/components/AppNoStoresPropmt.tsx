import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import { Box, Typography, Button } from '@mui/material';

export const  AppNoStoresPropmt = () => {
    return <Box>
        <Typography variant="body1">Seems like you don't have a seller account...</Typography>
        <Typography variant="h5">Open a new store to start selling!</Typography>
    </ Box>
}