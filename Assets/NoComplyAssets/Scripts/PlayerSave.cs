using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerSave
{
	//our total collected coins
	int coins;
	//our high score
	int score;
	//Our map index
	int mapIndex;
	//our character index
	int charIndex;

	//our constructor
	public PlayerSave()
	{
		coins = 0;
		score = 0;
		mapIndex = 0;
		charIndex = 0;
	}

	//Set and get methods
	public void setCoins(int coin)
	{
		coins = coin;
	}
	
	public int getCoins()
	{
		return coins;
	}

	public void setScore(int s)
	{
		score = s;
	}

	public int getScore()
	{
		return score;
	}

	public void setMapIndex(int i)
	{
		mapIndex = i;
	}

	public int getMapIndex()
	{
		return mapIndex;
	}

	public void setCharIndex(int i)
	{
		charIndex = i;
	}

	public int getCharIndex()
	{
		return charIndex;
	}


}
