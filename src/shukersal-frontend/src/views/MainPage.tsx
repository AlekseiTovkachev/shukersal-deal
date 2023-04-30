import { AppBar } from '../components/AppBar';
import { AppMainPageContainer } from '../components/AppMainPageContainer';
import { Outlet } from 'react-router-dom';

export const MainPage = () => {
    return <AppMainPageContainer>
        <AppBar>
            
        </AppBar>
        <Outlet />
    </AppMainPageContainer>
}