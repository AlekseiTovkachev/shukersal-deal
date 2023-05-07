
import { Box, Button, Typography } from '@mui/material';
import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

export const AppBarLoginButton = () => {
    const navigate = useNavigate();

    const handleClick = useCallback(() => {
        navigate('/login');
    }, []);

    return <Box display='flex' flexDirection='column'>
        {/* <Typography variant='caption'>Welcome</Typography> */}
        <Button
            color='secondary'
            variant='contained'
            onClick={handleClick}
        >
            Login
        </Button>
    </Box>
};