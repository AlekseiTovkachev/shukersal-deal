import { styled, Box } from '@mui/material';

export const AppPageScrollableContainer = styled(Box)(({ theme }) => ({
    width: '100%',
    height: '100%',
    overflowY: 'auto',
    display: 'flex',
    flexDirection: 'column',
    backgroundColor: theme.palette.background.default
}));