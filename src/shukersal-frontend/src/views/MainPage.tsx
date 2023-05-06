import { Outlet } from 'react-router-dom';

import { Grid, Box } from '@mui/material';

import { AppBar } from '../components/AppBar';
import { AppMainPageContainer } from '../components/AppMainPageContainer';
import { AppFooter } from '../components/AppFooter/AppFooter';
import { AppFooterText } from '../components/AppFooter/AppFooterText';
import { AppFooterTitle } from '../components/AppFooter/AppFooterTitle';
import { AppPageScrollableContainer } from '../components/AppPageScrollableContainer';
import { FlexSpacer } from '../components/FlexSpacer';
//import { AppPagePaddingBox } from '../components/AppPagePaddingBox';

export const MainPage = () => {
    return <AppMainPageContainer>
        <AppBar>

        </AppBar>
        <AppPageScrollableContainer>
            <Box sx={(theme) => ({
                p: 2,
                boxSizing: 'border-box',
                [theme.breakpoints.up('sm')]: {
                    px: 10,
                },
                [theme.breakpoints.up('md')]: {
                    px: 20,
                },
                [theme.breakpoints.up('lg')]: {
                    px: 25,
                },
                [theme.breakpoints.up('xl')]: {
                    px: 30,
                },
            })}><Outlet /></Box>
            <FlexSpacer />
            <AppFooter>
                <Grid container spacing={3}>
                    <Grid item xs={6} display="flex" flexDirection="column">
                        <AppFooterTitle>Follow Us</AppFooterTitle>
                        {/* <AppFooterText>Facebook</AppFooterText>
                    <AppFooterText>Instagram</AppFooterText> */}
                        <AppFooterText linkProps={{ href: 'https://github.com/AlekseiTovkachev/shukersal-deal/' }}>Github</AppFooterText>
                        <AppFooterText></AppFooterText>
                    </Grid>
                    <Grid item xs={6} display="flex" flexDirection="column">
                        <AppFooterTitle>HELP</AppFooterTitle>
                        <AppFooterText>FAQ</AppFooterText>
                        <AppFooterText>Technical Support</AppFooterText>
                        <AppFooterText>FAQ</AppFooterText>
                    </Grid>
                </Grid>
            </AppFooter>
        </AppPageScrollableContainer>
    </AppMainPageContainer>
}