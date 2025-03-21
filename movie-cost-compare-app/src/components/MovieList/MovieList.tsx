import React, { useState, useEffect, useCallback } from "react";

import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";

import { devApi } from "../../App";
import { IMovieSource } from "../../models/IMovieSource";
import { getAxios } from "../../services/AxiosService";
import { CircularProgress, responsiveFontSizes } from "@mui/material";
import { IMovieCatalogueItem } from "../../models/IMovieCatalogueItem";
import { MovieListItem } from "../MovieListItem/MovieListItem";

interface IMovieListProps {}

export const MovieList = (props: IMovieListProps) => {
  const [sources, setSources] = useState<IMovieSource[]>([]);
  const [catalogue, setCatalogue] = useState<IMovieCatalogueItem[]>([]);
  const [hasFetchedCat, setHasFetchedCat] = useState<boolean>(false);

  let moviesList: IMovieCatalogueItem[] = [];

  const fetchCatalogue = useCallback(async (id: number) => {
    let ax = getAxios();
    const response = await ax.get<IMovieCatalogueItem[]>(
      devApi + `compare/catalogue/${id}`
    );
    if (response.status !== 200) {
      console.error(response);
      throw new Error("failed to fetch catalogue.");
    }
    return response.data;
  }, []);

  const fetchSources = useCallback(async () => {
    let ax = getAxios(10);
    const response = await ax.get<IMovieSource[]>(devApi + "compare/sources");
    if (response.status !== 200) {
      throw new Error("failed to fetch providers.");
    }
    // const data: IMovieSource[] = await response.json();
    setSources(response.data);
  }, []);

  useEffect(() => {
    fetchSources().catch(console.error);
  }, []);

  useEffect(() => {
    const fetchAllCatalogues = async () => {
      sources.forEach(async (source: IMovieSource) => {
        let movies: IMovieCatalogueItem[] = [];
        await fetchCatalogue(source.sourceId)
          .then((response) => {
            response.forEach((movie: IMovieCatalogueItem) => {
              movie.imagePool = [movie.poster];
              movies.push(movie);
            });
            setCatalogue((catalogue) => [...catalogue, ...movies]);
            setHasFetchedCat(true);
          })
          .catch(console.error);
      });
    };
    fetchAllCatalogues().catch(console.error);
  }, [sources]);

  moviesList = catalogue;
  // useEffect(() => {
  if (hasFetchedCat) {
    moviesList = [];
    let newList: IMovieCatalogueItem[] = [];
    catalogue.forEach((m: IMovieCatalogueItem) => {
      let i = moviesList.findIndex(
        (x) => x.title.toLowerCase() === m.title.toLocaleLowerCase()
      );
      if (i < 0) {
        moviesList.push(m);
      } else if (i >= 0) {
        m.imagePool.push(moviesList[i].poster);
      }
    });

    setCatalogue(moviesList);
    setHasFetchedCat(false);
  }
  return (
    <TableContainer component={Paper}>
      <Table sx={{ minWidth: 650 }} aria-label="movie price comparison list">
        <TableHead>
          <TableRow key="header">
            <TableCell></TableCell>
            <TableCell align="right">Title</TableCell>
            <TableCell align="right">Year</TableCell>
            {sources.map((ms: IMovieSource) => (
              <TableCell key={ms.sourceName} align="right">
                {ms.sourceName}
              </TableCell>
            ))}
          </TableRow>
        </TableHead>

        {moviesList.length > 0 ? (
          moviesList.map((movie: IMovieCatalogueItem) => (
            <MovieListItem
              movieCatalogueItem={movie}
              movieSources={sources}
            ></MovieListItem>
          ))
        ) : (
          <CircularProgress />
        )}
      </Table>
    </TableContainer>
  );
};
