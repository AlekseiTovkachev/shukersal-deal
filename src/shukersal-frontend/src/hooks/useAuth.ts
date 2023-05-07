import { useCallback } from 'react';
import { useAppDispatch } from './useAppDispatch';
import { useAppSelector } from './useAppSelector';
import { LoginFormFields } from '../types/formTypes';
import { login } from '../redux/authSlice';

export const useAuth = () => {
    const dispatch = useAppDispatch();
    const currentMemberData = useAppSelector((state) => state.auth.data);
    const isLoading = useAppSelector((state) => state.auth.isLoading);
    const error = useAppSelector((state) => state.auth.error);

    const loginCallback = useCallback(async (formData: LoginFormFields) => {
        const response = await dispatch(login(formData));
        if (response.meta.requestStatus === 'fulfilled') {
            return true;
        }
        return false;

    }, [dispatch]);

    return {
        login: loginCallback,
        currentMemberData: currentMemberData,
        isLoggedIn: !!currentMemberData,
        isLoading: isLoading,
        error: error,
    };
}