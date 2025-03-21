import React, { useState, useEffect } from "react";
import "./MovieListItem.css";

import TableRow from "@mui/material/TableRow";
import TableCell from "@mui/material/TableCell";
import { IMovieCatalogueItem } from "../../models/IMovieCatalogueItem";
import { IMovieSource } from "../../models/IMovieSource";
import { getAxios } from "../../services/AxiosService";
import { devApi } from "../../App";
import { Card, CardContent, Paper } from "@mui/material";
import { MovieListItemPrice } from "../MovieListItemPrice/MovieListItemPrice";

interface IMovieListItemProps {
  movieCatalogueItem: IMovieCatalogueItem;
  movieSources: IMovieSource[];
}

export const MovieListItem = (props: IMovieListItemProps) => {
  const [imgFailed, setImgFailed] = useState<boolean>(false);
  const [imgIndex, setImageIndex] = useState<number>(0);
  const [needsUpdate, setNeedsUpdate] = useState<boolean>(false);
  const [img, setImage] = useState<string>(
    props.movieCatalogueItem.imagePool[imgIndex]
  );

  useEffect(() => {
    setImageIndex((index) => index + 1);
  }, [needsUpdate]);

  useEffect(() => {
    if (imgIndex > props.movieCatalogueItem.imagePool.length) {
      setImgFailed(true);
      return;
    }
    setImage(props.movieCatalogueItem.imagePool[imgIndex]);
  }, [props.movieCatalogueItem]);

  return (
    <TableRow
      key={props.movieCatalogueItem.title}
      sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
    >
      <TableCell align="right" component="th" scope="row">
        {imgFailed ? (
          <p></p>
        ) : (
          <img
            className="cover-img"
            onError={(i) => {
              i.currentTarget.style.display = "none";
              setNeedsUpdate(true);
            }}
            src={img}
          ></img>
        )}
      </TableCell>
      <TableCell align="right">{props.movieCatalogueItem.title}</TableCell>
      <TableCell align="right">{props.movieCatalogueItem.year}</TableCell>

      {props.movieSources.map((ms: IMovieSource) => (
        <TableCell align="right">
          <MovieListItemPrice
            vendorId={ms.sourceId}
            movieId={props.movieCatalogueItem.id}
          ></MovieListItemPrice>
        </TableCell>
      ))}
    </TableRow>
  );
};
