import { Box, Paper } from '@mui/material';
import { Outlet } from 'react-router-dom';
import { AppLogoFadeIn } from '../../components/AppLogo/AppLogoFadeIn';

// LoginPage component
export const LoginPage = () => {
    return (
        <Box sx={{
            width: '100%',
            height: '100%',
            display: 'flex',
            flexDirection: 'column',
            justifyContent: 'center',
            alignItems: 'center'
        }}>
            <Paper sx={(theme) => ({
                display: 'flex',
                flexDirection: 'column',
                boxSizing: 'border-box',
                p: 2,
                backgroundColor: theme.palette.primary.dark,
                [theme.breakpoints.down('sm')]: { // only xs
                    width: '100%',
                    height: '100%',
                    borderRadius: 0
                },
                [theme.breakpoints.up('sm')]: {
                    width: 500,
                    height: '80%'
                },
            })}>
                <Box sx={(theme) => ({ 
                    width: '100%',
                    boxSizing: 'border-box', 
                    px: 8,
                    py: 2
                })}>
                    <AppLogoFadeIn sx={{
                        width: '100%',
                        height: 'auto',
                    }} />
                </Box>
                <Box sx={(theme) => ({
                    width: '100%', 
                    height: '100%',
                    [theme.breakpoints.down('sm')]: {
                        pb: 4
                    },
                    })}>
                <Outlet />
                </Box>
            </Paper>
        </Box>
    )
}
