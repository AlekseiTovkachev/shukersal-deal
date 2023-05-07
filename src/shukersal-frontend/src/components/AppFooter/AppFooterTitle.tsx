import { Typography, TypographyProps, Link, LinkProps } from '@mui/material';

interface AppFooterTitleProps {
    children?: React.ReactNode,
    typographyProps?: TypographyProps,
    //linkProps?: LinkProps
}

export const AppFooterTitle = ({ children, typographyProps = {}, /* linkProps = {} */ }: AppFooterTitleProps) =>
    <Typography variant="h5" color={(theme) => theme.palette.secondary.contrastText} {...typographyProps}>
        {/* <Link href="#" underline="none" {...linkProps}> */}
        {children}
        {/* </Link> */}
    </Typography>;
