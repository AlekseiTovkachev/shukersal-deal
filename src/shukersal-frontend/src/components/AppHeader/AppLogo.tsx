
import { Box, Typography } from "@mui/material";

const companyLogoUrl = '/shukersal_logo.svg'
export const AppLogo = () => {
    return <Box
        component="img"
        src={companyLogoUrl}
        sx={{
            height: '100%',
            width: 'auto',
            background: 'rgba(0,0,0,0)'
        }}
    />
};