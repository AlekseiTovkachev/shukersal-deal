export const devWarnNotLoggedIn = () => {
    console.warn('This component requires the user to be logged in.');
};

export const errorToString = (data: any): string => {
    if (typeof data === 'string') {
      return data;
    }
    
    return data?.title ?? 'Server Error';
  };