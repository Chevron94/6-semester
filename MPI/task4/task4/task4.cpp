
#include "stdafx.h"
#include <mpi.h>
#include <random>
#include <time.h>
#include <iostream>
#include <string>
#pragma warning(disable : 4996)

using namespace std;

#define m 6
#define n 5

void PrintMatrix(int (*arr)[n], int num)
{
	char message[200] = "Matrix ";
	strncat_s(message,(num==0 ? "before" : "after "),6);
	strncat_s(message,":\n",3);
	char buf[5];
	for (int i=0;i<m;i++)
	{
		for (int j=0;j<n;j++)
		{
			_itoa_s(arr[i][j],buf,10);
			strncat_s(message,buf,strlen(buf));
			strncat_s(message," ",1);
		}
		strncat_s(message,"\n",2);
	}
	printf("%s\n",message);
}


int _tmain(int argc, char* argv[])
{
	int myrank, tag=0;
	MPI_Status status;
	MPI_Init(&argc,&argv);
	MPI_Comm_rank(MPI_COMM_WORLD,&myrank);

	int rowlen = n/2;
	int rowcnt = m/2;

	int blocklength[m];
	int displacement[m];

	for(int i = 0; i<rowcnt; i++)
	{
		blocklength[i] = rowlen;
		displacement[i] = n -rowlen + i*n;
	}
	for(int i = rowcnt; i<m; i++)
	{
		blocklength[i] = n - rowlen;
		displacement[i] =  i*n;
	}
	
	MPI_Datatype type;
	MPI_Type_indexed(m,blocklength,displacement,MPI_INT,&type);
	MPI_Type_commit(&type);
	int Matr[m][n];
	for(int i = 0; i < m; i ++) 
		for(int j = 0; j < n; j ++)
			Matr[i][j]=myrank;
	if (0==myrank)
	{
		MPI_Send(&Matr[0][0],1,type,1,tag,MPI_COMM_WORLD);        
	}
	else if (1==myrank)
	{
		PrintMatrix(Matr, 0); 
		MPI_Recv(&Matr[0][0],1,type,0,tag,MPI_COMM_WORLD,&status);	 
		PrintMatrix(Matr,1); 
	}
	MPI_Type_free(&type);

	MPI_Finalize();
	return 0;
}