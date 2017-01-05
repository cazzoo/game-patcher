// RFUpdater is a game module updater that allows teams to create, manage and get folders synched.
// Copyright (C) 2016 - //Racing-France//
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;

namespace RFUpdater
{
	public class SettingsList
	{
		private static INIFile iniSettings;
		public List<Setting> applicationSettings;

		public SettingsList ()
		{
			applicationSettings = new List<Setting> ();
			iniSettings = new INIFile (Globals.settingFile, true, false);
			initialize (ref applicationSettings);
		}

		private List<Setting> getListSettingsEmpty ()
		{
			var emptySettings = new List<Setting> ();

			emptySettings.Add (new Setting ("Test", "Value", "DEFAULT"));

			return emptySettings;
		}

		/**
		 * Initialize list with values found in ini file or default if not found
		 */
		public void initialize (ref List<Setting> applicationSettings)
		{
			applicationSettings = getListSettingsEmpty ();
			foreach (Setting setting in applicationSettings)
			{
				string iniValue = iniSettings.GetValue ("DEFAULT", setting.Name, string.Empty);
				if (!setting.Value.Equals (iniValue)) {
					setting.Value = iniValue;
				}
			}
		}
	}
}
