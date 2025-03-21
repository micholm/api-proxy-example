import { CircularProgress } from '@mui/material';
import React, { useState, useEffect, useCallback } from 'react';
import { getAxios } from '../../services/AxiosService';
import { devApi } from '../../App';

import { IPriceReqest } from '../../models/IPriceRequest';
interface IMovieListItemPriceProps {
    vendorId:number;
    movieId:string;
}



export const MovieListItemPrice = (props:IMovieListItemPriceProps) => {
    const [price, setPrice] = useState<number>(0.0);


    const fetchPrice = () => {
        let ax = getAxios(20);
        let d:IPriceReqest =  {vendorId:props.vendorId, movieId:props.movieId}
        ax.post<number>(devApi + "compare/price", d)
        .then((response) => {
            setPrice(response.data);
        })
        .catch(() => {setPrice(-1)});
        
        // const data: IMovieSource[] = await response.json();
    }
    
    
    useEffect(() => {
      fetchPrice();
    }, []);
    
    return (
        price > 0 ? <p>${price}</p> : price < 0 ? <p>-</p> : <CircularProgress /> 
    )
}
