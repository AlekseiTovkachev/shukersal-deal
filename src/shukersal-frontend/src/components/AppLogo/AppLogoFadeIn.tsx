
import { Box, SxProps, Typography } from "@mui/material";
import { AppLogo } from "./AppLogo";

const companyLogoUrl = '/shukersal_logo.svg'
interface AppLogoFadeInProps {
    sx?: SxProps
}

export const AppLogoFadeIn = ({ sx = {} }: AppLogoFadeInProps) => {
    return <AppLogo sx={{
        opacity: 0,
        transform: 'translateY(20px)',
        animation: 'fadeIn 2s cubic-bezier(.25,.46,.45,.94) forwards',
        '@keyframes fadeIn': {
            to: {
                opacity: 1,
                transform: 'translateY(0)',
            },
        },
        ...sx
    }} />
};