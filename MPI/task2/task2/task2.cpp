#include "stdafx.h"
#include "mpi.h"
#include <string.h>
#include <cstdlib>
#include <cmath>
#include <iostream>	
#include <iomanip>
#pragma warning(disable : 4996)

float Sum_log(float x,float eps)
{
	float sum_pred = x;
	float sum = x;
	float sqr_x = x*x;
	int i = 2;
	float cur = x;

	do
	{
		sum_pred = sum;
		cur = (-1)*cur*sqr_x*(2*i-3)/((float)((2*i-2)));
		sum+=cur/(2*i-1);
		i++;

	}
	while (abs(sum - sum_pred) > eps);

	return sum;
}

int _tmain(int argc, char* argv[])
{
	int myrank,numprocs,n;
	float e;
	float *sendbuf_x;  
	float *readbuf_x;  
	float *sendbuf_res;
	float *readbuf_res;
	int buf_size;


	MPI_Init(&argc,&argv);
	MPI_Comm_size(MPI_COMM_WORLD, &numprocs);
	MPI_Comm_rank(MPI_COMM_WORLD,&myrank);

	if(myrank==0)
	{
		float A=0,B=0;
		std::cout <<"Enter A:\n";
		std::cin >> A;

		std::cout <<"Enter B:\n";
		std::cin >> B;

		std::cout <<"Enter epsilon:\n";
		do
			std::cin >> e;
		while (e<=0);

		std::cout <<"Enter n:\n";
		
		do
			std::cin >> n;
		while (n<2); 

		sendbuf_x=new float[n];
		float ABN = (float)(B-A)/(n-1); 
		for (int i=0; i<n; i++)
			sendbuf_x[i]=A+i*ABN;    
		buf_size = n/numprocs+(int)(n % numprocs != 0);
	}
	else sendbuf_x=NULL;

	MPI_Bcast(&e,1,MPI_FLOAT,0,MPI_COMM_WORLD);
	MPI_Bcast(&buf_size,1,MPI_INT,0,MPI_COMM_WORLD);

	readbuf_x=new float[buf_size];
	sendbuf_res=new float[2*(buf_size)];
	readbuf_res = new float[2*numprocs*buf_size];


	MPI_Scatter(sendbuf_x,buf_size,MPI_FLOAT,readbuf_x,buf_size,MPI_FLOAT,0,MPI_COMM_WORLD);
	
	float x;
	for (int i=0;i<buf_size;i++) 
	{
		
		x=readbuf_x[i];
		if (abs(x) <=1)
		{
			sendbuf_res[2*i]=Sum_log(x,e); 
			sendbuf_res[2*i+1]=log(x+sqrt(x*x+1)); 
		}
	}
		
	MPI_Gather(sendbuf_res,2*(buf_size),MPI_FLOAT,readbuf_res,2*(buf_size),MPI_FLOAT,0,MPI_COMM_WORLD);
	
	if (myrank==0)
	{
		std::cout<<"x\t\tSumma\t\tExact value\n";
		for (int i=0; i<n;i++)
			std::cout << std::fixed << std::setprecision(6) << sendbuf_x[i] <<'\t'<<readbuf_res[2*i]<<'\t'<<readbuf_res[2*i+1]<<'\n';
	}
	MPI_Finalize();
	system("Pause");
	return 0;
}