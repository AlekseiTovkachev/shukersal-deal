import axios from 'axios';
import { store } from '../redux/store';
import { API_URL } from '../_configuration';

export const apiClient = axios.create({
    baseURL: API_URL,
    headers: (() => {
        const currentMemberData = store.getState().auth.data;
        if (currentMemberData) {
            return {
                Authorization: 'Token ' + currentMemberData.token
            };
        }
        return {};
    })()
});