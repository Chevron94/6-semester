// task1.cpp : Defines the entry point for the console application.
//x1..x(k/2) = A
//x((k/2)+1)..xn = B

#include "stdafx.h"
#include <mpi.h>
#include <random>
#include <time.h>
#include <iostream>

using namespace std;

int _tmain(int argc, char* argv[])
{
	int m;
	int rank;
	int numproc;
	int tag = 0;
	MPI_Status status;

	MPI_Init(&argc, &argv);
	MPI_Comm_size(MPI_COMM_WORLD, &numproc);
	MPI_Comm_rank(MPI_COMM_WORLD, &rank);
	if (rank == 0)
	{
		int N=0;
		int m_;
		int cnt;
		int res = 0;
		int tmp_res;
		cout << "Enter N\n";
		do
			cin >> N;
		while (N<=0);

		m = N/((numproc-1)/2); // размерность кусков вектора 
		if (N%((numproc-1)/2) != 0)
		{
			m_ = m+1;
			cnt = N%((numproc-1)/2);
		}
		else 
		{
			cnt = 0;
		}
		for (int i=1;(i<=numproc/2);i++)
		{
			if (cnt > 0)
			{
				MPI_Send(&m_, 1, MPI_INT, i, tag, MPI_COMM_WORLD);
				MPI_Send(&m_, 1, MPI_INT, i+numproc/2, tag, MPI_COMM_WORLD);
				cnt --;
			}else 
			{
				MPI_Send(&m, 1, MPI_INT, i, tag, MPI_COMM_WORLD);
				MPI_Send(&m, 1, MPI_INT, i+numproc/2, tag, MPI_COMM_WORLD);
			}
		}

		for (int i=numproc/2+1;(i<numproc);i++)
		{
			MPI_Recv(&tmp_res, 1, MPI_INT, i, tag, MPI_COMM_WORLD, &status);
			res += tmp_res;
		}
		cout << "Result: " << res << '\n';
	}
	else
	{
		MPI_Recv(&m, 1, MPI_INT, 0, tag, MPI_COMM_WORLD, &status);
		int *x;
		x = new int[m];
		srand(time(NULL)+rank);
		char str[100]="Process: ";
		char buf[5];
		itoa(rank,buf,10);
		strncat(str,buf,strlen(buf));
		strncat(str," array: ",8);

		for (int i = 0; i<m; i++)
		{
			x[i] = x[i]=rand()%(13*13*rank);
			itoa(x[i],buf,10);
			strncat(str,buf,strlen(buf));
			strncat(str,", ",2);
		}
		cout << str << '\n';
		if (rank<=numproc/2)
			MPI_Send(x,m,MPI_INT,rank+numproc/2,tag,MPI_COMM_WORLD);
		else
		{
			int *y;
			y = new int[m];
			int result = 0;
			MPI_Recv(y,m,MPI_INT,rank-numproc/2,tag,MPI_COMM_WORLD,&status);
			for (int i = 0; i<m; i++)
			{
				result+=x[i]*y[i];
			}
			MPI_Send(&result, 1, MPI_INT, 0, tag, MPI_COMM_WORLD);
		}
	}

	MPI_Finalize();
	system("pause");
	return 0;
}

