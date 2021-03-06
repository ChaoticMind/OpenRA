#region Copyright & License Information
/*
 * Copyright 2007-2015 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using System;
using Eluant;
using OpenRA.Mods.Common.Traits;
using OpenRA.Scripting;
using OpenRA.Traits;

namespace OpenRA.Mods.Common.Scripting
{
	[ScriptPropertyGroup("MissionObjectives")]
	public class MissionObjectiveProperties : ScriptPlayerProperties
	{
		readonly MissionObjectives mo;

		public MissionObjectiveProperties(ScriptContext context, Player player)
			: base(context, player)
		{
			mo = player.PlayerActor.Trait<MissionObjectives>();
		}

		[ScriptActorPropertyActivity]
		[Desc("Add a primary mission objective for this player. The function returns the " +
			"ID of the newly created objective, so that it can be referred to later.")]
		public int AddPrimaryObjective(string description)
		{
			return mo.Add(Player, description, ObjectiveType.Primary);
		}

		[ScriptActorPropertyActivity]
		[Desc("Add a secondary mission objective for this player. The function returns the " +
			"ID of the newly created objective, so that it can be referred to later.")]
		public int AddSecondaryObjective(string description)
		{
			return mo.Add(Player, description, ObjectiveType.Secondary);
		}

		[ScriptActorPropertyActivity]
		[Desc("Mark an objective as completed.  This needs the objective ID returned " +
			"by AddObjective as argument.  When this player has completed all primary " +
			"objectives, (s)he has won the game.")]
		public void MarkCompletedObjective(int id)
		{
			if (id < 0 || id >= mo.Objectives.Count)
				throw new LuaException("Objective ID is out of range.");

			mo.MarkCompleted(Player, id);
		}

		[ScriptActorPropertyActivity]
		[Desc("Mark an objective as failed.  This needs the objective ID returned " +
			"by AddObjective as argument.  Secondary objectives do not have any " +
			"influence whatsoever on the outcome of the game.")]
		public void MarkFailedObjective(int id)
		{
			if (id < 0 || id >= mo.Objectives.Count)
				throw new LuaException("Objective ID is out of range.");

			mo.MarkFailed(Player, id);
		}

		[ScriptActorPropertyActivity]
		[Desc("Returns true if the objective has been successfully completed, false otherwise.")]
		public bool IsObjectiveCompleted(int id)
		{
			if (id < 0 || id >= mo.Objectives.Count)
				throw new LuaException("Objective ID is out of range.");

			return mo.Objectives[id].State == ObjectiveState.Completed;
		}

		[ScriptActorPropertyActivity]
		[Desc("Returns true if the objective has been failed, false otherwise.")]
		public bool IsObjectiveFailed(int id)
		{
			if (id < 0 || id >= mo.Objectives.Count)
				throw new LuaException("Objective ID is out of range.");

			return mo.Objectives[id].State == ObjectiveState.Failed;
		}

		[ScriptActorPropertyActivity]
		[Desc("Returns the description of an objective.")]
		public string GetObjectiveDescription(int id)
		{
			if (id < 0 || id >= mo.Objectives.Count)
				throw new LuaException("Objective ID is out of range.");

			return mo.Objectives[id].Description;
		}

		[ScriptActorPropertyActivity]
		[Desc("Returns the type of an objective.")]
		public string GetObjectiveType(int id)
		{
			if (id < 0 || id >= mo.Objectives.Count)
				throw new LuaException("Objective ID is out of range.");

			return mo.Objectives[id].Type == ObjectiveType.Primary ? "Primary" : "Secondary";
		}

		[ScriptActorPropertyActivity]
		[Desc("Returns true if this player has lost all units/actors that have " +
			"the MustBeDestroyed trait (according to the short game option).")]
		public bool HasNoRequiredUnits()
		{
			return Player.HasNoRequiredUnits();
		}
	}
}
