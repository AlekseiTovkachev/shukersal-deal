import { styled, Box } from '@mui/material';

export const AppFooter = styled(Box)(({ theme }) => ({
    position: 'relative',
    display: 'flex',
    justifyContent: 'center',
    width: '100%',
    padding: theme.spacing(2),
    boxSizing: 'border-box',
    //height: 50,

    backgroundColor: theme.palette.secondary.dark,
    //boxShadow: '0px 2px 4px rgba(0, 0, 0, 0.2)',
}));