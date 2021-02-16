list.of.packages <- c("httr")
noninst.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
if(length(noninst.packages)) install.packages(noninst.packages)

lapply(list.of.packages, require, character.only = TRUE)

#################################################
# classe: "WebClient"